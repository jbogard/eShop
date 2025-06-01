using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Features.Orders.CancelOrder;

public static class CancelOrderApi
{
    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CancelOrderAsync(
        CancelOrderRequest request,
        [AsParameters] OrderServices services)
    {
        var isSuccess = await services.Mediator.Send(request);
        
        if (!isSuccess)
        {
            return TypedResults.Problem(detail: "Cancel order failed to process.",
                statusCode: (int?)HttpStatusCode.BadRequest);
        }
        
        return TypedResults.Ok();
    }
}