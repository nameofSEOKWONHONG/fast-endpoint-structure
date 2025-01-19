using Infrastructure.Base;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Feature.Infra.Message;

public class NotificationHub : HubBase<NotificationHub>
{
    public NotificationHub(ILogger<NotificationHub> logger) : base(logger)
    {
    }

    public async Task SendNotification(string user, string notification)
    {
        // 특정 사용자에게 알림 전송
        await Clients.User(user).SendAsync("ReceiveNotification", notification);
    }

    public async Task BroadcastNotification(string notification)
    {
        // 모든 사용자에게 알림 전송
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }
}