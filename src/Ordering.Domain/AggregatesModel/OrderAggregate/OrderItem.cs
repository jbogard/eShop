using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderItem
    : Entity
{
    [Required]
    public string ProductName { get; set; }
    
    public string PictureUrl { get; set;}
    
    public decimal UnitPrice { get; set;}
    
    public decimal Discount { get; set; }
    
    public int Units { get; set; }

    public int ProductId { get; set; }
}
