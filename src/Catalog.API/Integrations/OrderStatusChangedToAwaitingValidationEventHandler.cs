using eShop.Catalog.Contracts;
using eShop.Ordering.Contracts;

namespace eShop.Catalog.API.Integrations;

public class OrderStatusChangedToAwaitingValidationEventHandler : IHandleMessages<OrderAwaitingValidationEvent>
{
    private readonly CatalogContext _catalogContext;

    public OrderStatusChangedToAwaitingValidationEventHandler(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task Handle(OrderAwaitingValidationEvent message, IMessageHandlerContext context)
    {
        foreach (var orderStockItem in message.OrderItems)
        {
            var catalogItem = await _catalogContext.CatalogItems.FindAsync([orderStockItem.ProductId], context.CancellationToken);
            var hasStock = catalogItem!.AvailableStock >= orderStockItem.Quantity;

            if (!hasStock)
            {
                await context.Publish(new OrderStockRejectedEvent
                {
                    OrderId = message.OrderId
                });
                return;
            }
        }

        await context.Publish(new OrderStockConfirmedEvent
        {
            OrderId = message.OrderId
        });
    }
}
