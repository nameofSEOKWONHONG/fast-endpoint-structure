using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public abstract class ServiceBase<TSelf> : ServiceCore<TSelf>
    where TSelf : class
{
    protected ILogger Logger;
    protected ISessionContext SessionContext;

    protected ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext) : base(logger)
    {
        SessionContext = sessionContext;
    }
}

public abstract class ServiceBase<TSelf, TRequest, TResult> : ServiceCore<TSelf>
    where TSelf : class
{
    protected ILogger Logger;
    protected ISessionContext SessionContext;

    protected ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext) : base(logger)
    {
        SessionContext = sessionContext;
    }
    
    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public abstract class ServiceBase<TSelf, TDbContext, TRequest, TResult> : ServiceBase<TSelf, TRequest, TResult>
    where TSelf : class
{
    protected TDbContext DbContext;
    protected ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext, TDbContext dbContext) : base(logger, sessionContext)
    {
        DbContext = dbContext;
    }
}

public abstract class ServiceBatchBase<TSelf, TService, TRequest, TResult> : ServiceBase<TSelf, TRequest, TResult>
    where TSelf : class
{
    protected readonly TService Service;

    protected ServiceBatchBase(ILogger<TSelf> logger, ISessionContext sessionContext, TService service) : base(logger, sessionContext)
    {
        Service = service;
    }
}