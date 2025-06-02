namespace eShop.Ordering.Domain.Events;

public class BuyerAndPaymentMethodVerifiedDomainEvent : INotification
{
    public Buyer Buyer { get; set; }
    public PaymentMethod Payment { get; set; }
    public int OrderId { get; set; }
}
