namespace eShop.Ordering.Domain.Events;

public class OrderAwaitingValidationDomainEvent : INotification
{
    public Order Order { get; set; }
}
