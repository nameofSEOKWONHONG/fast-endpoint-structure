using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public class RepositoryBase<TSelf> : DisposeBase
    where TSelf : class
{
    protected ILogger Logger;
    protected ISessionContext SessionContext;
    
    protected RepositoryBase(ILogger<TSelf> logger, ISessionContext sessionContext)
    {
        Logger = logger;
        SessionContext = sessionContext;
    }
}

public abstract class RepositoryBase<TSelf, TDbContext, TRequest, TResult> : RepositoryBase<TSelf> 
    where TSelf : class
    where TDbContext : DbContext
{
    protected TDbContext DbContext;
    protected RepositoryBase(ILogger<TSelf> logger, ISessionContext sessionContext, TDbContext dbContext) : base(logger, sessionContext)
    {
        DbContext = dbContext;
    }

    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}