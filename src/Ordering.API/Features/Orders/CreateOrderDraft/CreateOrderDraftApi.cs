namespace eShop.Ordering.API.Features.Orders.CreateOrderDraft;

public static class CreateOrderDraftApi
{
    public static async Task<OrderDraftModel> CreateOrderDraftAsync(CreateOrderDraftRequest request,
        [AsParameters] OrderServices services)
    {
        var model = await services.Mediator.Send(request);

        return model;
    }
}