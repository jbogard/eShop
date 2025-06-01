namespace eShop.Ordering.API.DTOs;

public record CreateOrderDraftModel(string BuyerId, IEnumerable<BasketItem> Items) : IRequest<OrderDraftModel>;
