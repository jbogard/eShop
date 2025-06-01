using System.Net;
using eShop.Ordering.API.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

public static class OrdersApi
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders").HasApiVersion(1.0);

        api.MapPut("/cancel", CancelOrderAsync);
        api.MapPut("/ship", ShipOrderAsync);
        api.MapGet("{orderId:int}", GetOrderAsync);
        api.MapGet("/", GetOrdersByUserAsync);
        api.MapGet("/cardtypes", GetCardTypesAsync);
        api.MapPost("/draft", CreateOrderDraftAsync);
        api.MapPost("/", CreateOrderAsync);

        return api;
    }

    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CancelOrderAsync(
        CancelOrderModel model,
        [AsParameters] OrderServices services)
    {
        var order = await services.DbContext.Orders.FindAsync(model.OrderNumber);

        if (order != null)
        {
            await services.DbContext.Orders.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
        }

        if (order == null)
        {
            return TypedResults.Problem(detail: "Cancel order failed to process.",
                statusCode: (int?)HttpStatusCode.BadRequest);
        }

        order.OrderStatus = OrderStatus.Cancelled;

        await services.DbContext.SaveChangesAsync();

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> ShipOrderAsync(
        ShipOrderModel model,
        [AsParameters] OrderServices services)
    {
        var order = await services.DbContext.Orders.FindAsync(model.OrderNumber);

        if (order != null)
        {
            await services.DbContext.Orders.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
        }

        if (order == null)
        {
            return TypedResults.BadRequest("Cannot find order");
        }

        order.OrderStatus = OrderStatus.Shipped;

        await services.DbContext.SaveChangesAsync();

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok<OrderDto>, NotFound>> GetOrderAsync(int orderId,
        [AsParameters] OrderServices services)
    {
        try
        {
            var order = await services.DbContext
                .Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is null)
                throw new KeyNotFoundException();

            var dto = new OrderDto
            {
                OrderNumber = order.Id,
                Date = order.OrderDate,
                Description = order.Description,
                City = order.Address.City,
                Country = order.Address.Country,
                State = order.Address.State,
                Street = order.Address.Street,
                Zipcode = order.Address.ZipCode,
                Status = order.OrderStatus.ToString(),
                Total = OrderManager.GetTotal(order),
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = oi.ProductName,
                    Units = oi.Units,
                    UnitPrice = (double)oi.UnitPrice,
                    PictureUrl = oi.PictureUrl
                }).ToList()
            };
            return TypedResults.Ok(dto);
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }

    public static async Task<Ok<IEnumerable<OrderSummaryDto>>> GetOrdersByUserAsync(
        [AsParameters] OrderServices services)
    {
        var userId = services.IdentityService.GetUserIdentity();
        IEnumerable<OrderSummaryDto> orders = await services.DbContext
            .Orders
            .Where(o => o.Buyer.IdentityGuid == userId)
            .Select(o => new OrderSummaryDto
            {
                OrderNumber = o.Id,
                Date = o.OrderDate,
                Status = o.OrderStatus.ToString(),
                Total = (double)o.OrderItems.Sum(oi => oi.UnitPrice * oi.Units)
            })
            .ToListAsync();

        return TypedResults.Ok(orders);
    }

    public static async Task<Ok<IEnumerable<CardTypeDto>>> GetCardTypesAsync([AsParameters] OrderServices services)
    {
        IEnumerable<CardTypeDto> cardTypes = await services.DbContext
            .CardTypes
            .Select(c => new CardTypeDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return TypedResults.Ok(cardTypes);
    }

    public static Task<OrderDraftModel> CreateOrderDraftAsync(CreateOrderDraftModel model,
        [AsParameters] OrderServices services)
    {
        var order = new Order();
        var orderItems = model.Items.Select(i => i.ToOrderItemDTO()).ToList();
        foreach (var item in orderItems)
        {
            OrderManager.AddOrderItem(order, item.ProductId, item.ProductName, item.UnitPrice, item.Discount,
                item.PictureUrl, item.Units);
        }

        return Task.FromResult(OrderDraftModel.FromOrder(order));
    }

    public static async Task<Results<Ok, BadRequest<string>>> CreateOrderAsync(
        NewOrderModel model,
        [AsParameters] OrderServices services)
    {
        var address = new Address
        {
            Street = model.Street,
            City = model.City,
            State = model.State,
            Country = model.Country,
            ZipCode = model.ZipCode
        };
        var order = new Order { OrderStatus = OrderStatus.Submitted, OrderDate = DateTime.UtcNow, Address = address };
        foreach (var item in model.Items)
        {
            OrderManager.AddOrderItem(order, item.ProductId, item.ProductName, item.UnitPrice, item.Discount,
                item.PictureUrl);
        }

        await services.DbContext.Orders.AddAsync(order);

        await services.DbContext.SaveChangesAsync();

        var cardTypeId = model.CardTypeId != 0 ? model.CardTypeId : 1;
        var buyer = await services.DbContext.Buyers
            .Include(b => b.PaymentMethods)
            .SingleOrDefaultAsync(b => b.IdentityGuid == model.UserId);
        var buyerExisted = buyer is not null;

        if (!buyerExisted)
        {
            buyer = new Buyer { IdentityGuid = model.UserId, Name = model.UserName };
        }

        var payment = buyer.PaymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, model.CardNumber, model.CardExpiration));

        if (payment == null)
        {
            payment = new PaymentMethod
            {
                CardTypeId = cardTypeId,
                Alias = $"Payment Method on {DateTime.UtcNow}",
                CardNumber = model.CardNumber,
                SecurityNumber = model.CardSecurityNumber,
                CardHolderName = model.CardHolderName,
                Expiration = model.CardExpiration
            };

            buyer.PaymentMethods.Add(payment);
        }

        if (buyerExisted)
        {
            services.DbContext.Buyers.Update(buyer);
        }
        else
        {
            services.DbContext.Buyers.Add(buyer);
        }

        await services.DbContext.SaveChangesAsync();
        
        // Update order details with buyer information
        order.Buyer = buyer;
        order.PaymentId = payment.Id;
        
        services.DbContext.Orders.Update(order);

        await services.DbContext.SaveChangesAsync();
        
        return TypedResults.Ok();
    }
}
