using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public class JEndpoint<TSelf, TRequest, TResult, TDbContext> : Endpoint<TRequest, TResult>
    where TSelf : class
    where TDbContext : DbContext
{
    protected ILogger Logger { get; }
    protected TDbContext DbContext { get; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dbContext"></param>
    public JEndpoint(ILogger<TSelf> logger, TDbContext dbContext)
    {
        Logger = logger;
        DbContext = dbContext;
    }
}