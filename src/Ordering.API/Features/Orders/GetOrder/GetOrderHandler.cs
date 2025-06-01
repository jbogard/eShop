using eShop.Ordering.API.DTOs;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.API.Features.Orders.GetOrder;

public class GetOrderHandler : IRequestHandler<GetOrderRequest, OrderDto>
{
    private readonly OrderingContext _dbContext;

    public GetOrderHandler(OrderingContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OrderDto> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _dbContext
            .Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId);

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

        return dto;
    }
}
