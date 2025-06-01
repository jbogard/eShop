using eShop.Ordering.API.DTOs;

namespace eShop.Ordering.API.Features.Orders.GetOrder;

public record OrderDto
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string Status { get; init; }
    public string Description { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Zipcode { get; init; }
    public string Country { get; init; }
    public List<OrderItemDto> OrderItems { get; set; }
    public decimal Total { get; set; }
}