using eShop.Ordering.API.DTOs;
using eShop.Ordering.Domain.Commands;

namespace eShop.Ordering.API.Features.Orders.CreateOrder;

public record CreateOrderRequest(
    string UserId,
    string UserName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    int CardTypeId,
    string Buyer,
    List<BasketItem> Items) : IRequest, ICreateOrderCommand
{
    IEnumerable<ICreateOrderCommand.IOrderItem> ICreateOrderCommand.Items => Items;
}
