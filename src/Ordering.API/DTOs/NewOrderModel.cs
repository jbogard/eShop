namespace eShop.Ordering.API.DTOs;

public record NewOrderModel(
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
    List<NewOrderModel.OrderItem> Items)
{
    public record OrderItem
    {
        public int ProductId { get; init; }

        public string ProductName { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal Discount { get; init; }

        public int Units { get; init; }

        public string PictureUrl { get; init; }
    }
}
