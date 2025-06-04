using eShop.Ordering.Contracts;
using eShop.Ordering.Domain.Events;
using NServiceBus.TransactionalSession;

namespace eShop.Ordering.API.Features.Orders.DomainEventHandlers;

public class PublishOrderAwaitingValidationFromDomainEventHandler : INotificationHandler<OrderAwaitingValidationDomainEvent>
{
    private readonly ITransactionalSession _messageSession;

    public PublishOrderAwaitingValidationFromDomainEventHandler(ITransactionalSession messageSession)
    {
        _messageSession = messageSession;
    }
    
    public async Task Handle(OrderAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new OrderAwaitingValidationEvent
        {
            OrderId = notification.Order.Id,
            OrderItems = notification.Order.OrderItems.Select(item => new OrderAwaitingValidationEvent.OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Units
                }
            ).ToList()
        };
        
        await _messageSession.Publish(message, cancellationToken);
    }
}
