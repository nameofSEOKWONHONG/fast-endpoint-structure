using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Base;

public class HubBase<T> : Hub
{
    private readonly ILogger<T> _logger;
    public HubBase(ILogger<T> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}