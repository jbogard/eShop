using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderItem
    : Entity
{
    protected OrderItem()
    {
        
    }
    
    public OrderItem(string productName, string pictureUrl, decimal unitPrice, decimal discount, int units, int productId)
    {
        ProductName = productName;
        PictureUrl = pictureUrl;
        UnitPrice = unitPrice;
        Discount = discount;
        Units = units;
        ProductId = productId;
    }

    [Required]
    public string ProductName { get; init; }
    
    public string PictureUrl { get; init;}
    
    public decimal UnitPrice { get; init;}
    
    public decimal Discount { get; private set; }
    
    public int Units { get; private set; }

    public int ProductId { get; init; }

    public decimal GetPrice()
    {
        return Units * UnitPrice;
    }

    public void ApplyDiscount(decimal discount)
    {
        Discount = discount;
    }
    
    public void AddUnits(int units)
    {
        Units += units;
    }
}
