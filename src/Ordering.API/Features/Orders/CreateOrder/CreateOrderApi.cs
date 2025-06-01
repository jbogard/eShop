using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Features.Orders.CreateOrder;

public static class CreateOrderApi
{
    public static async Task<Results<Ok, BadRequest<string>>> CreateOrderAsync(
        CreateOrderRequest request,
        [AsParameters] OrderServices services)
    {
        await services.Mediator.Send(request);
        
        return TypedResults.Ok();
    }
}