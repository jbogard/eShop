using eShop.Ordering.Domain.Commands;

namespace eShop.Ordering.API.DTOs;

public class BasketItem : ICreateOrderCommand.IOrderItem
{
    public string Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal OldUnitPrice { get; init; }
    public int Quantity { get; init; }
    public string PictureUrl { get; init; }
}

