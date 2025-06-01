namespace eShop.Ordering.API.Features.Orders.ShipOrder;

public record ShipOrderRequest(int OrderNumber) : IRequest<bool>;
