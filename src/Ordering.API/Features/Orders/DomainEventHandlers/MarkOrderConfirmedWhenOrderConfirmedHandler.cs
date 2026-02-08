using eShop.Catalog.Contracts;

namespace eShop.Ordering.API.Features.Orders.DomainEventHandlers;

public class MarkOrderConfirmedWhenOrderConfirmedHandler : IHandleMessages<OrderStockConfirmedEvent>
{
    public Task Handle(OrderStockConfirmedEvent message, IMessageHandlerContext context)
    {
        // Load order, mark as stock confirmed
        return Task.CompletedTask;
    }
}
