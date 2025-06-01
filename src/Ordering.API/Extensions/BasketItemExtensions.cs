using eShop.Ordering.API.DTOs;

namespace eShop.Ordering.API.Extensions;

public static class BasketItemExtensions
{
    public static IEnumerable<OrderDraftModel.OrderItem> ToOrderItemsDTO(this IEnumerable<BasketItem> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDTO();
        }
    }

    public static OrderDraftModel.OrderItem ToOrderItemDTO(this BasketItem item)
    {
        return new OrderDraftModel.OrderItem()
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            PictureUrl = item.PictureUrl,
            UnitPrice = item.UnitPrice,
            Units = item.Quantity
        };
    }
}
