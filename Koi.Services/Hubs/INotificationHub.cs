namespace Koi.Services.Hubs
{
    public interface INotificationHub
    {
        Task ReceiveMessage(string user, string message);
    }
}
