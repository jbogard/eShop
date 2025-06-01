using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Features.Orders.GetOrders;

public static class GetOrdersApi
{
    public static async Task<Ok<IEnumerable<OrderSummaryDto>>> GetOrdersByUserAsync(
        [AsParameters] OrderServices services)
    {
        var orders = await services.Mediator.Send(new GetOrdersRequest());

        return TypedResults.Ok(orders);
    }
}