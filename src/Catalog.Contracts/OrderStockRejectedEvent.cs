using NServiceBus;

namespace eShop.Catalog.Contracts;

public class OrderStockRejectedEvent : IEvent
{
    public int OrderId { get; set; }
}
