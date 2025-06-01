using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order
    : Entity
{
    private readonly List<OrderItem> _orderItems = new();

    protected Order()
    {
    }

    private Order(Address address)
    {
        OrderStatus = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;
        Address = address;
    }

    public static Order NewDraft()
    {
        return new Order();
    }

    public static Order NewOrder(Address address)
    {
        return new Order(address);
    }

    public DateTime OrderDate { get; private set; }

    [Required]
    public Address Address { get; private set; }

    public int? BuyerId { get; private set; }

    public Buyer Buyer { get; private set; }

    public OrderStatus OrderStatus { get; private set; }
    
    public string Description { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public int? PaymentId { get; private set; }

    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount,
        string pictureUrl, int units = 1)
    {
        var existingOrderForProduct = OrderItems
            .SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.Discount)
            {
                existingOrderForProduct.ApplyDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(productName, pictureUrl, unitPrice, discount, units, productId);
            
            _orderItems.Add(orderItem);
        }
    }

    public decimal GetTotal() 
        => OrderItems.Sum(orderItem => orderItem.GetPrice());

    public void Ship()
    {
        OrderStatus = OrderStatus.Shipped;
        Description = "The order was shipped.";
    }

    public void Cancel()
    {
        OrderStatus = OrderStatus.Cancelled;
        Description = $"The order was cancelled.";
    }

    public void AssignBuyerDetails(Buyer buyer, PaymentMethod payment)
    {
        Buyer = buyer;
        PaymentId = payment.Id;
    }
}
