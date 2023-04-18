
using Microsoft.AspNetCore.SignalR;

namespace ChessProject.App.Hubs;

public class GameHub : Hub
{
    private static int connectedUsers = 0;

    public override Task OnConnectedAsync()
    {
        if (connectedUsers < 2)
        {
            connectedUsers++;
            return base.OnConnectedAsync();
        }
        else
        {
            return Task.CompletedTask;
        }
    }
    
    public override Task OnDisconnectedAsync(Exception exception)
    {
        connectedUsers--;
        return base.OnDisconnectedAsync(exception);
    }
    public async Task JoinGame(string playerName)
    {
        if (connectedUsers > 2)
        {
            await Clients.Caller.SendAsync("GameFull");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, "GameRoom");

        await Clients.OthersInGroup("GameRoom").SendAsync("PlayerJoined", playerName);
    }

    public async Task SendMove(string move, string fen)
    {
        await Clients.OthersInGroup("GameRoom").SendAsync("PlayerMove", move, fen);
    }
    
    public async Task LeaveGame()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "GameRoom");
        await Clients.OthersInGroup("GameRoom").SendAsync("PlayerLeft");
    }
}
