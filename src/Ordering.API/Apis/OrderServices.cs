public class OrderServices(
    OrderingContext dbContext,
    IIdentityService identityService)
{
    public OrderingContext DbContext { get; } = dbContext;
    public IIdentityService IdentityService { get; } = identityService;
}
