namespace eShop.Ordering.API.Features.Orders.GetOrder;

public record GetOrderRequest : IRequest<OrderDto>
{
    public int OrderId { get; init; }
}
