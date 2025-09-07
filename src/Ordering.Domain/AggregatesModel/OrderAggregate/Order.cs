using System.ComponentModel.DataAnnotations;
using eShop.Ordering.Domain.Commands;
using eShop.Ordering.Domain.Events;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order
    : Entity
{
    private readonly List<OrderItem> _orderItems = new();

    protected Order() { }
    
    public Order(ICreateOrderCommand request)
    {
        var address = new Address
        {
            Street = request.Street,
            City = request.City,
            State = request.State,
            Country = request.Country,
            ZipCode = request.ZipCode
        };
        Address = address;
        OrderStatus = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;

        foreach (var item in request.Items)
        {
            AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, 0m, item.PictureUrl, item.Quantity);
        }

        AddDomainEvent(new OrderStartedDomainEvent
        {
            Order = this,
            CardSecurityNumber = request.CardSecurityNumber,
            CardTypeId = request.CardTypeId,
            CardExpiration = request.CardExpiration,
            CardNumber = request.CardNumber,
            UserId = request.UserId,
            UserName = request.UserName,
            CardHolderName = request.CardHolderName
        });
    }
    
    public static Order NewDraft()
    {
        return new Order();
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
        
        AddDomainEvent(new OrderAwaitingValidationDomainEvent { Order = this });
    }
    
    public void MarkOrderAsStockConfirmed()
    {
        OrderStatus= OrderStatus.StockConfirmed;
        Description = "All the items were confirmed with available stock.";
    }
}
