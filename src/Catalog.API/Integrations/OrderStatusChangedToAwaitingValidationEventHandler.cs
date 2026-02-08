using eShop.Ordering.Contracts;

namespace eShop.Catalog.API.Integrations;

public class OrderStatusChangedToAwaitingValidationEventHandler : IHandleMessages<OrderAwaitingValidationEvent>
{
    private readonly CatalogContext _catalogContext;

    public OrderStatusChangedToAwaitingValidationEventHandler(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public Task Handle(OrderAwaitingValidationEvent message, IMessageHandlerContext context)
    {
        // Foreach order item, find the stock item and verify the stock details
        // If any do not have stock, publish OrderStockRejectedEvent
        // If all have stock, publish OrderStockConfirmedEvent
        
        return Task.CompletedTask;
    }
}
