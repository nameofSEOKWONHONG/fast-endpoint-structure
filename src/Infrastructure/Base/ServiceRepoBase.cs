using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public abstract class ServiceRepoBase<TSelf, TRepository, TRequest, TResult> : ServiceBase<TSelf>
    where TSelf : class
    where TRepository : class
{
    protected TRepository Repository;
    
    protected ServiceRepoBase(ILogger<TSelf> logger, ISessionContext sessionContext, TRepository repository) : base(logger, sessionContext)
    {
        Repository = repository;
    }
    
    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}