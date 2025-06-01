using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class PaymentMethod : Entity
{
    [Required]
    public string Alias { get; set; }
    [Required]
    public string CardNumber { get; set; }
    public string SecurityNumber { get; set; }
    [Required]
    public string CardHolderName { get; set; }
    public DateTime Expiration { get; set; }

    public int CardTypeId { get; set; }
    public CardType CardType { get; set; }

    public int BuyerId { get; set; }

    public Buyer Buyer { get; set; }
    
    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
    {
        return CardTypeId == cardTypeId
               && CardNumber == cardNumber
               && Expiration == expiration;
    }
}
