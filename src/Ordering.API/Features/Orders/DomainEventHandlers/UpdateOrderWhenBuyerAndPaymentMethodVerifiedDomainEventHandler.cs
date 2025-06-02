using eShop.Ordering.Domain.Events;

namespace eShop.Ordering.API.Features.Orders.DomainEventHandlers;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler
    : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
    private readonly OrderingContext _orderingContext;

    public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(OrderingContext orderingContext)
    {
        _orderingContext = orderingContext;
    }
    
    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderingContext.Orders.FindAsync(notification.OrderId)
            ?? throw new OrderingDomainException("Invalid Order ID");
        
        // Update order details with buyer information
        order.AssignBuyerDetails(notification.Buyer, notification.Payment);
        
        await _orderingContext.SaveEntitiesAsync(cancellationToken);

    }
}
