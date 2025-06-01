using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Features.Orders.ShipOrder;

public static class ShipOrdersApi
{
    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> ShipOrderAsync(
        ShipOrderRequest request,
        [AsParameters] OrderServices services)
    {
        var isSuccess = await services.Mediator.Send(request);

        if (!isSuccess)
        {
            return TypedResults.BadRequest("Cannot find order");
        }

        return TypedResults.Ok();
    }
}