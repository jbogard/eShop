namespace eShop.Ordering.API.Features.Orders.CreateOrder;

public class CreateOrderHandler(OrderingContext dbContext) : IRequestHandler<CreateOrderRequest>
{
    public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var address = new Address
        {
            Street = request.Street,
            City = request.City,
            State = request.State,
            Country = request.Country,
            ZipCode = request.ZipCode
        };
        var order = Order.NewOrder(address: address);
        foreach (var item in request.Items)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, 0m,
                item.PictureUrl, item.Quantity);
        }

        await dbContext.Orders.AddAsync(order);

        await dbContext.SaveEntitiesAsync();

        var cardTypeId = request.CardTypeId != 0 ? request.CardTypeId : 1;
        var buyer = await dbContext.Buyers
            .Include(b => b.PaymentMethods)
            .SingleOrDefaultAsync(b => b.IdentityGuid == request.UserId);
        var buyerExisted = buyer is not null;

        if (!buyerExisted)
        {
            buyer = new Buyer(identityGuid: request.UserId, name: request.UserName);
        }

        var payment = buyer.VerifyOrAddPaymentMethod(cardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpiration);

        if (buyerExisted)
        {
            dbContext.Buyers.Update(buyer);
        }
        else
        {
            dbContext.Buyers.Add(buyer);
        }

        await dbContext.SaveEntitiesAsync();
        
        order.AssignBuyerDetails(buyer, payment);

        dbContext.Orders.Update(order);

        await dbContext.SaveEntitiesAsync();
    }
}
