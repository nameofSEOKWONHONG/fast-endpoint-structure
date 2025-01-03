using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public abstract class ServiceBase<TSelf> : DisposeBase
    where TSelf : class
{
    protected ILogger Logger;
    protected ISessionContext SessionContext;

    protected ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext)
    {
        Logger = logger;
        SessionContext = sessionContext;
    }
}

public abstract class ServiceBase<TSelf, TDbContext> : ServiceBase<TSelf> 
    where TSelf : class
{
    protected TDbContext DbContext;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    protected ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext, TDbContext dbContext) : base(logger, sessionContext)
    {
        DbContext = dbContext;
    }
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    protected ServiceBase(ILogger<TSelf> logger, TDbContext dbContext): this(logger,null, dbContext)
    {
        
    }    
}

public abstract class ServiceBase<TSelf, TDbContext, TRequest, TResult> : ServiceBase<TSelf, TDbContext>
    where TSelf : class
{
    protected ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext, TDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
        DbContext = dbContext;
    }

    protected ServiceBase(ILogger<TSelf> logger, TDbContext dbContext) : base(logger, dbContext)
    {
        DbContext = dbContext;
    }
    
    public abstract Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}