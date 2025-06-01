namespace Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

public static class OrderManager
{
    public static void AddOrderItem(Order order, int productId, string productName, decimal unitPrice, decimal discount,
        string pictureUrl, int units = 1)
    {
        var existingOrderForProduct = order.OrderItems
            .SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.Discount)
            {
                existingOrderForProduct.Discount = discount;
            }

            existingOrderForProduct.Units += units;
        }
        else
        {
            var orderItem = new OrderItem
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = unitPrice,
                Discount = discount,
                Units = units,
                PictureUrl = pictureUrl,
            };
            order.OrderItems.Add(orderItem);
        }
    }

    public static decimal GetTotal(Order order) 
        => order.OrderItems.Sum(orderItem => orderItem.Units * orderItem.UnitPrice);
}