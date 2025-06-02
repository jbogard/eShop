namespace eShop.Ordering.Domain.Commands;

public interface ICreateOrderCommand
{
    string City { get; }
    string Street { get; }
    string State { get; }
    string Country { get; }
    string ZipCode { get; }
    string CardNumber { get; }
    string CardHolderName { get; }
    DateTime CardExpiration { get; }
    string CardSecurityNumber { get; }
    int CardTypeId { get; }
    string Buyer { get; }
    IEnumerable<IOrderItem> Items { get; }
    string UserId { get; }
    string UserName { get; }
    
    public interface IOrderItem
    {
        int ProductId { get; }
        string ProductName { get; }
        decimal UnitPrice { get; }
        int Quantity { get; }
        string PictureUrl { get; }
    }
}
