namespace eShop.Ordering.API.Features.Orders.GetOrder;

public record OrderItemDto
{
    public string ProductName { get; init; }
    public int Units { get; init; }
    public double UnitPrice { get; init; }
    public string PictureUrl { get; init; }
}