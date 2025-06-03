using eShop.Catalog.Contracts;

namespace eShop.Ordering.API.Features.Orders.IntegrationEventHandlers;

public class MarkOrderCancelledWhenStockRejectedHandler : IHandleMessages<OrderStockRejectedEvent>
{
    private readonly OrderingContext _orderingContext;

    public MarkOrderCancelledWhenStockRejectedHandler(OrderingContext orderingContext)
    {
        _orderingContext = orderingContext;
    }
    public async Task Handle(OrderStockRejectedEvent message, IMessageHandlerContext context)
    {
        var order = await _orderingContext.Orders.FindAsync([message.OrderId], context.CancellationToken);

        order.Cancel();

        await _orderingContext.SaveEntitiesAsync(context.CancellationToken);
    }
}
