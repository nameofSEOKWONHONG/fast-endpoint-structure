using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public class ServiceBase<TSelf> : DisposeBase
    where TSelf : class
{
    protected ILogger Logger;
    protected ISessionContext SessionContext;
    public ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext)
    {
        Logger = logger;
        SessionContext = sessionContext;
    }
}

public class ServiceBase<TSelf, TDbContext> : ServiceBase<TSelf> 
    where TSelf : class
    where TDbContext : DbContext
{
    protected TDbContext DbContext;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public ServiceBase(ILogger<TSelf> logger, ISessionContext sessionContext, TDbContext dbContext) : base(logger, sessionContext)
    {
        DbContext = dbContext;
    }
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public ServiceBase(ILogger<TSelf> logger, TDbContext dbContext): this(logger,null, dbContext)
    {
        
    }    
}