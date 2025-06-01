using eShop.Ordering.API.DTOs;

namespace eShop.Ordering.API.Features.CardTypes.GetCardTypes;

public class GetCardTypesHandler(OrderingContext dbContext) 
    : IRequestHandler<GetCardTypesRequest, IEnumerable<CardTypeDto>>
{
    public async Task<IEnumerable<CardTypeDto>> Handle(GetCardTypesRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<CardTypeDto> cardTypes = await dbContext
            .CardTypes
            .Select(c => new CardTypeDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return cardTypes;
    }
}
