using AutoMapper;
using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Koi.Services.Hubs;
using Koi.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Koi.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> notificationHubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHubContext = notificationHubContext;
        }

        // Push notification to a specific user or to everyone if ReceiverId is null
        public async Task PushNotification(Notification notification)
        {
            // Map and create new notification
            var newNotification = new Notification
            {
                Title = notification.Title,
                Body = notification.Body,
                ReceiverId = notification.ReceiverId, // If null, it will be a broadcast message
                IsRead = false,
                Url = notification.Url,
                Type = notification.Type ?? "USER", // Default to 'USER' if not provided
            };

            // Save notification to DB
            await _unitOfWork.NotificationRepository.AddAsync(newNotification);
            await _unitOfWork.SaveChangeAsync();

            // Push notification to SignalR clients
            if (notification.ReceiverId == null)
            {
                // If ReceiverId is null, push to all clients
                await _notificationHubContext.Clients.All.SendAsync("ReceiveNotification", notification.Title, notification.Body);
            }
            else
            {
                // Push notification to a specific user
                await _notificationHubContext.Clients.User(notification.ReceiverId.ToString())
                    .SendAsync("ReceiveNotification", notification.Title, notification.Body);
            }
        }

        // Push notification to users with "Manager" role
        public async Task PushNotificationToManager(Notification notification)
        {
            var newNotification = new Notification
            {
                Title = notification.Title,
                Body = notification.Body,
                ReceiverId = null, // Broadcast to managers
                IsRead = false,
                Url = notification.Url,
                Type = "ROLE", // Indicate this notification is for a role
            };

            // Save notification to DB
            await _unitOfWork.NotificationRepository.AddAsync(newNotification);
            await _unitOfWork.SaveChangeAsync();

            // Push notification to the "Manager" SignalR group
            await _notificationHubContext.Clients.Group("Manager").SendAsync("ReceiveNotification", notification.Title, notification.Body);
        }

        // Retrieve notifications for the current user
        public async Task<List<Notification>> GetNotifications()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetListByUserId();
            return notifications;
        }

        // Mark all notifications as read for the current user
        public async Task<List<Notification>> ReadAllNotification()
        {
            var notifications = await _unitOfWork.NotificationRepository.ReadAllNotification();
            return notifications;
        }

        // Get the number of unread notifications for the current user
        public async Task<int> GetUnreadNotificationQuantity()
        {
            var unreadCount = await _unitOfWork.NotificationRepository.GetUnreadNotificationQuantity();
            return unreadCount;
        }
    }
}
