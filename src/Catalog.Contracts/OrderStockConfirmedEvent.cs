
using NServiceBus;

namespace eShop.Catalog.Contracts;

public class OrderStockConfirmedEvent : IEvent
{
    public int OrderId { get; set; }
}
