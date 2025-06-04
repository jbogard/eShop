using NServiceBus.TransactionalSession;

namespace eShop.Ordering.API.Infrastructure.Behaviors;

public class TransactionSessionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ITransactionalSession _transactionalSession;

    public TransactionSessionBehavior(ITransactionalSession transactionalSession)
    {
        _transactionalSession = transactionalSession;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _transactionalSession.Open(new SqlPersistenceOpenSessionOptions(), cancellationToken);
        
        var response = await next();

        await _transactionalSession.Commit(cancellationToken);
        
        return response;
    }
}
