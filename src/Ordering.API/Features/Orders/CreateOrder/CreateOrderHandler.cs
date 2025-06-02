namespace eShop.Ordering.API.Features.Orders.CreateOrder;

public class CreateOrderHandler(OrderingContext dbContext) : IRequestHandler<CreateOrderRequest>
{
    public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order(request);

        await dbContext.Orders.AddAsync(order);

        await dbContext.SaveEntitiesAsync();
    }
}
