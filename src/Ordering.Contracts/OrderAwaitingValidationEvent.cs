using NServiceBus;

namespace eShop.Ordering.Contracts;

public class OrderAwaitingValidationEvent : IEvent
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public int OrderId { get; set; }
    public required List<OrderItem> OrderItems { get; set; }
}
