using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class Buyer
    : Entity
{
    public Buyer()
    {
    }

    public Buyer(string identityGuid, string name)
    {
        IdentityGuid = identityGuid;
        Name = name;
    }

    [Required] public string IdentityGuid { get; set; }

    public string Name { get; set; }

    public ICollection<PaymentMethod> PaymentMethods { get; } = new List<PaymentMethod>();

    public ICollection<Order> Orders { get; } = new List<Order>();

    public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, string cardNumber, string cardSecurityNumber,
        string cardHolderName, DateTime cardExpiration)
    {
        var payment = PaymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, cardExpiration));

        if (payment == null)
        {
            payment = new PaymentMethod
            {
                CardTypeId = cardTypeId,
                Alias = $"Payment Method on {DateTime.UtcNow}",
                CardNumber = cardNumber,
                SecurityNumber = cardSecurityNumber,
                CardHolderName = cardHolderName,
                Expiration = cardExpiration
            };

            PaymentMethods.Add(payment);
        }

        return payment;
    }
}
