using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.API.Features.Orders.CreateOrderDraft;

public class CreateOrderDraftHandler : IRequestHandler<CreateOrderDraftRequest, OrderDraftModel>
{
    public Task<OrderDraftModel> Handle(CreateOrderDraftRequest request, CancellationToken cancellationToken)
    {
        var order = new Order();
        var orderItems = request.Items.Select(i => i.ToOrderItemDTO()).ToList();
        foreach (var item in orderItems)
        {
            OrderManager.AddOrderItem(order, item.ProductId, item.ProductName, item.UnitPrice, item.Discount,
                item.PictureUrl, item.Units);
        }

        return Task.FromResult(OrderDraftModel.FromOrder(order));
    }
}
