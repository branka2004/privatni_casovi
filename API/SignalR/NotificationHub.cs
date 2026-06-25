using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class NotificationHub : Hub
{
    public async Task RegisterUser(int korisnikId)
    {
        UserConnections.Connections[korisnikId]
            = Context.ConnectionId;

        await Task.CompletedTask;
    }

    public override async Task OnDisconnectedAsync(
        Exception? exception)
    {
        var user = UserConnections.Connections
            .FirstOrDefault(x =>
                x.Value == Context.ConnectionId);

        if (user.Key != 0)
        {
            UserConnections.Connections.Remove(
                user.Key);
        }

        await base.OnDisconnectedAsync(exception);
    }
}