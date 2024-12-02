using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public interface IRepositoryBase
{
    
}

public class RepositoryBase<TSelf> : DisposeBase, IRepositoryBase
    where TSelf : class
{
    protected ILogger Logger;
    protected ISessionContext SessionContext;
    
    public RepositoryBase(ILogger<TSelf> logger, ISessionContext sessionContext)
    {
        Logger = logger;
        SessionContext = sessionContext;
    }
}

public class RepositoryBase<TSelf, TDbContext> : RepositoryBase<TSelf> 
    where TSelf : class
    where TDbContext : DbContext
{
    protected TDbContext DbContext;
    public RepositoryBase(ILogger<TSelf> logger, ISessionContext sessionContext, TDbContext dbContext) : base(logger, sessionContext)
    {
        DbContext = dbContext;
    }
} 