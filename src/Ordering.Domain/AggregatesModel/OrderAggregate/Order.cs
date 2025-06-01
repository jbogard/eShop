using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order
    : Entity
{
    public DateTime OrderDate { get; set; }

    [Required]
    public Address Address { get; set; }

    public int? BuyerId { get; set; }

    public Buyer Buyer { get; set; }

    public OrderStatus OrderStatus { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public int? PaymentId { get; set; }
}
