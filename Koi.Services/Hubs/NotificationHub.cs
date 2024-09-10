using Microsoft.AspNetCore.SignalR;

namespace Koi.Services.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendToAUser(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
