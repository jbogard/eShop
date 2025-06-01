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
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount,
                item.PictureUrl);
        }

        await dbContext.Orders.AddAsync(order);

        await dbContext.SaveChangesAsync();

        var cardTypeId = request.CardTypeId != 0 ? request.CardTypeId : 1;
        var buyer = await dbContext.Buyers
            .Include(b => b.PaymentMethods)
            .SingleOrDefaultAsync(b => b.IdentityGuid == request.UserId);
        var buyerExisted = buyer is not null;

        if (!buyerExisted)
        {
            buyer = new Buyer { IdentityGuid = request.UserId, Name = request.UserName };
        }

        var payment = buyer.PaymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, request.CardNumber, request.CardExpiration));

        if (payment == null)
        {
            payment = new PaymentMethod
            {
                CardTypeId = cardTypeId,
                Alias = $"Payment Method on {DateTime.UtcNow}",
                CardNumber = request.CardNumber,
                SecurityNumber = request.CardSecurityNumber,
                CardHolderName = request.CardHolderName,
                Expiration = request.CardExpiration
            };

            buyer.PaymentMethods.Add(payment);
        }

        if (buyerExisted)
        {
            dbContext.Buyers.Update(buyer);
        }
        else
        {
            dbContext.Buyers.Add(buyer);
        }

        await dbContext.SaveChangesAsync();
        
        // Update order details with buyer information
        order.Buyer = buyer;
        order.PaymentId = payment.Id;
        
        dbContext.Orders.Update(order);

        await dbContext.SaveChangesAsync();
    }
}
