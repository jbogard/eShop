public class OrderServices(
    OrderingContext dbContext,
    IIdentityService identityService,
    IMediator mediator)
{
    public OrderingContext DbContext { get; } = dbContext;
    public IIdentityService IdentityService { get; } = identityService;
    public IMediator Mediator { get; } = mediator;
}
