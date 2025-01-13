using System.Collections.Concurrent;
using Infrastructure.Base;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Feature.Infra.Message;

public class ChatHub : HubBase<ChatHub>
{
    public ChatHub(ILogger<ChatHub> logger) : base(logger)
    {
    }
    
    public override async Task OnConnectedAsync()
    {
        // 사용자가 연결되었을 때 사용자 ID를 로깅
        var userId = Context.User?.Identity?.Name;
        if (userId != null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Welcome {userId}!");
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessageToUser(string userId, string message)
    {
        // 특정 사용자에게 메시지 전송
        await Clients.User(userId).SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message);
    }

    public async Task BroadcastMessage(string userId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", userId, message);
    }
    
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has joined the group {groupName}.");
    }
    
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }
}