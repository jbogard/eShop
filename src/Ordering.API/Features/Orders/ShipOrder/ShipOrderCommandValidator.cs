namespace eShop.Ordering.API.Features.Orders.ShipOrder;

public class ShipOrderCommandValidator : AbstractValidator<ShipOrderRequest>
{
    public ShipOrderCommandValidator(ILogger<ShipOrderCommandValidator> logger)
    {
        RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
