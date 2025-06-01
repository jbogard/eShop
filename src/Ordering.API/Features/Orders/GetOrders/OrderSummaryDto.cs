namespace eShop.Ordering.API.Features.Orders.GetOrders;

public record OrderSummaryDto
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; }
    public decimal Total { get; init; }
}
