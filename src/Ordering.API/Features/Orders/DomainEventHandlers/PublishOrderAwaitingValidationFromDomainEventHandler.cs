using eShop.Ordering.Domain.Events;

namespace eShop.Ordering.API.Features.Orders.DomainEventHandlers;

public class PublishOrderAwaitingValidationFromDomainEventHandler : INotificationHandler<OrderAwaitingValidationDomainEvent>
{
    public Task Handle(OrderAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
