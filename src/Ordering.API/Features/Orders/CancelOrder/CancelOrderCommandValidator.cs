namespace eShop.Ordering.API.Features.Orders.CancelOrder;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderRequest>
{
    public CancelOrderCommandValidator(ILogger<CancelOrderCommandValidator> logger)
    {
        RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}
