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
        return Task.CompletedTask;
    }
}
