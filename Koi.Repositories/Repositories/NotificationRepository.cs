using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly KoiFarmShopDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        private readonly UserManager<User> _userManager;

        public NotificationRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claims, UserManager<User> userManager) : base(context, timeService, claims)

        {
            _context = context;
            _timeService = timeService;
            _claimsService = claims;
            _userManager = userManager;
        }

        public async Task<List<Notification>> ReadAllNotification()
        {
            var userId = _claimsService.GetCurrentUserId;
            if (userId == null)
            {
                throw new Exception("UserId are invalid or you are not login");
            }
            var notifications = await _context.Notifications.Where(x => x.ReceiverId == userId).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<int> GetUnreadNotificationQuantity()
        {
            var userId = _claimsService.GetCurrentUserId;
            if (userId == null)
            {
                throw new Exception("UserId are invalid or you are not login");
            }
            var result = await _context.Notifications.Where(x => x.ReceiverId == userId && x.IsRead == false).CountAsync();
            return result;
        }

        public async Task<List<Notification>> GetListByUserId()
        {
            // Get the current user ID from the claims service
            var userId = _claimsService.GetCurrentUserId;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User is not logged in or the user ID is invalid.");
            }

            // Find the user in the database
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Retrieve notifications for the user (global, user-specific, and role-based)
            var notifications = await _context.Notifications
                .Where(n => n.Type == "ALL"
                            || n.ReceiverId == user.Id
                            || (n.Type == "ROLE" && roles.Contains(n.Type)))
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications;
        }
    }
}
