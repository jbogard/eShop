using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Features.Orders.GetOrder;

public static class GetOrderApi
{
    public static async Task<Results<Ok<OrderDto>, NotFound>> GetOrderAsync(int orderId,
        [AsParameters] OrderServices services)
    {
        try
        {
            var request = new GetOrderRequest { OrderId = orderId };

            var dto = await services.Mediator.Send(request);
            
            return TypedResults.Ok(dto);
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }
}