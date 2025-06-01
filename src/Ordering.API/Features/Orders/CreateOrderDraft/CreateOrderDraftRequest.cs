using eShop.Ordering.API.DTOs;

namespace eShop.Ordering.API.Features.Orders.CreateOrderDraft;

public record CreateOrderDraftRequest(string BuyerId, IEnumerable<BasketItem> Items) : IRequest<OrderDraftModel>;
