namespace eShop.Ordering.API.Features.Orders.CancelOrder;

public record CancelOrderRequest(int OrderNumber) : IRequest<bool>;

