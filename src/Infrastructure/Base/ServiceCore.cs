using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public abstract class ServiceCore<TSelf> : DisposeBase
    where TSelf : class
{
    protected readonly ILogger Logger;
    protected ServiceCore(ILogger<TSelf> logger)
    {
        Logger = logger;
    }    
}