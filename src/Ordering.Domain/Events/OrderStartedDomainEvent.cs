namespace eShop.Ordering.Domain.Events;

public class OrderStartedDomainEvent : INotification
{
    public string CardNumber { get; set; }

    public string CardHolderName { get; set; }

    public DateTime CardExpiration { get; set; }

    public string CardSecurityNumber { get; set; }
    public int CardTypeId { get; set; }

    public string UserId { get; set; }
    
    public string UserName { get; set; }

    public Order Order { get; set; }
}
