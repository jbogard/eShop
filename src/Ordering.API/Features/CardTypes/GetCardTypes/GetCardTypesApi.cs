using eShop.Ordering.API.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Features.CardTypes.GetCardTypes;

public static class GetCardTypesApi
{
    public static async Task<Ok<IEnumerable<CardTypeDto>>> GetCardTypesAsync([AsParameters] OrderServices services)
    {
        var cardTypes = await services.Mediator.Send(new GetCardTypesRequest());
        
        return TypedResults.Ok(cardTypes);
    }
}