using Koi.BusinessObjects;

namespace Koi.Services.Interface
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotifications();

        Task<int> GetUnreadNotificationQuantity();

        Task PushNotification(Notification notification);

        Task PushNotificationToManager(Notification notification);

        Task<List<Notification>> ReadAllNotification();
    }
}
