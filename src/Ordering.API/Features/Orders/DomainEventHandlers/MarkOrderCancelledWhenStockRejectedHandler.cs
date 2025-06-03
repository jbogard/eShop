using eShop.Catalog.Contracts;

namespace eShop.Ordering.API.Features.Orders.DomainEventHandlers;

public class MarkOrderCancelledWhenStockRejectedHandler : IHandleMessages<OrderStockRejectedEvent>
{
    public Task Handle(OrderStockRejectedEvent message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}
