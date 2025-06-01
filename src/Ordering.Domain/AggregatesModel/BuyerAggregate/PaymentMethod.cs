using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class PaymentMethod : Entity
{
    [Required]
    public string Alias { get; init; }
    [Required]
    public string CardNumber { get; init; }
    public string SecurityNumber { get; init; }
    [Required]
    public string CardHolderName { get; init; }
    public DateTime Expiration { get; init; }

    public int CardTypeId { get; init; }
    public CardType CardType { get; init; }

    public int BuyerId { get; init; }

    public Buyer Buyer { get; init; }
    
    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
    {
        return CardTypeId == cardTypeId
               && CardNumber == cardNumber
               && Expiration == expiration;
    }
}
