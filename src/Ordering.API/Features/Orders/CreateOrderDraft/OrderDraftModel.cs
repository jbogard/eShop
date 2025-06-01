namespace eShop.Ordering.API.Features.Orders.CreateOrderDraft;

public record OrderDraftModel
{
    public IEnumerable<OrderItem> OrderItems { get; init; }
    public decimal Total { get; init; }

    public static OrderDraftModel FromOrder(Order order)
    {
        return new OrderDraftModel()
        {
            OrderItems = order.OrderItems.Select(oi => new OrderItem
            {
                Discount = oi.Discount,
                ProductId = oi.ProductId,
                UnitPrice = oi.UnitPrice,
                PictureUrl = oi.PictureUrl,
                Units = oi.Units,
                ProductName = oi.ProductName
            }),
            Total = order.GetTotal()
        };
    }
    
    public record OrderItem
    {
        public int ProductId { get; init; }

        public string ProductName { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal Discount { get; init; }

        public int Units { get; init; }

        public string PictureUrl { get; init; }
    }
}
