namespace eShop.Ordering.API.Features.Orders.CancelOrder;

public class CancelOrderHandler : IRequestHandler<CancelOrderRequest, bool>
{
    private readonly OrderingContext _orderingContext;

    public CancelOrderHandler(OrderingContext orderingContext)
    {
        _orderingContext = orderingContext;
    }

    public async Task<bool> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderingContext.Orders.FindAsync(request.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.OrderStatus = OrderStatus.Cancelled;
        orderToUpdate.Description = $"The order was cancelled.";

        await _orderingContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
