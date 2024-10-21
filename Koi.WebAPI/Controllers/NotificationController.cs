using Koi.BusinessObjects;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Get notifications
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetNotifications();
                return Ok(ApiResult<List<Notification>>.Succeed(notifications, "Get notifications successfully!"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // Get unread notifications quantity
        [HttpGet("unread-quantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnreadNotificationQuantity()
        {
            try
            {
                var quantity = await _notificationService.GetUnreadNotificationQuantity();
                return Ok(ApiResult<int>.Succeed(quantity, "Get unread notification quantity successfully!"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // Push a notification
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PushNotification([FromBody] Notification notification)
        {
            try
            {
                await _notificationService.PushNotification(notification);
                return Ok(ApiResult<object>.Succeed(null, "Notification pushed successfully!"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // Push a notification to managers
        [HttpPost("push-to-manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PushNotificationToManager([FromBody] Notification notification)
        {
            try
            {
                await _notificationService.PushNotificationToManager(notification);
                return Ok(ApiResult<object>.Succeed(null, "Notification pushed to managers successfully!"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // Mark all notifications as read
        [HttpPost("read-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAllNotification()
        {
            try
            {
                await _notificationService.ReadAllNotification();
                return Ok(ApiResult<object>.Succeed(null, "All notifications marked as read!"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }
    }
}
