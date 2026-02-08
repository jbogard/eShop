using eShop.Catalog.Contracts;

namespace eShop.Ordering.API.Features.Orders.DomainEventHandlers;

public class MarkOrderCancelledWhenStockRejectedHandler : IHandleMessages<OrderStockRejectedEvent>
{
    public Task Handle(OrderStockRejectedEvent message, IMessageHandlerContext context)
    {
        // Load order, mark as rejected
        return Task.CompletedTask;
    }
}
