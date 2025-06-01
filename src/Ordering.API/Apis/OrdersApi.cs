using eShop.Ordering.API.Features.CardTypes.GetCardTypes;
using eShop.Ordering.API.Features.Orders.CancelOrder;
using eShop.Ordering.API.Features.Orders.CreateOrder;
using eShop.Ordering.API.Features.Orders.CreateOrderDraft;
using eShop.Ordering.API.Features.Orders.GetOrder;
using eShop.Ordering.API.Features.Orders.GetOrders;
using eShop.Ordering.API.Features.Orders.ShipOrder;

public static class OrdersApi
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders").HasApiVersion(1.0);

        api.MapPut("/cancel", CancelOrderApi.CancelOrderAsync);
        api.MapPut("/ship", ShipOrdersApi.ShipOrderAsync);
        api.MapGet("{orderId:int}", GetOrderApi.GetOrderAsync);
        api.MapGet("/", GetOrdersApi.GetOrdersByUserAsync);
        api.MapGet("/cardtypes", GetCardTypesApi.GetCardTypesAsync);
        api.MapPost("/draft", CreateOrderDraftApi.CreateOrderDraftAsync);
        api.MapPost("/", CreateOrderApi.CreateOrderAsync);

        return api;
    }
}
