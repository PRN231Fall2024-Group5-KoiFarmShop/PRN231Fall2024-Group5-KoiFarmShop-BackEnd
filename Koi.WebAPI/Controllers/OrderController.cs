using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _paymentService;
        private readonly IVnPayService _vnPayService;
        private readonly IClaimsService _claimsService;

        public OrderController(IOrderService paymentService, IVnPayService vnPayService, IClaimsService claimsService)
        {
            _paymentService = paymentService;
            _vnPayService = vnPayService;
            _claimsService = claimsService;
        }

        /// <summary>
        /// Get a list of full order
        /// </summary>
        /// <response code="200">Returns a list</response>
        /// /// <response code="400">Failed</response>
        /// <response code="401">User Not Found</response>
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var result = await _paymentService.GetOrdersAsync();
                return Ok(ApiResult<List<OrderDTO>>.Succeed(result, "Get list order Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var result = await _paymentService.GetOrderByIdAsync(id);
                return Ok(ApiResult<OrderDTO>.Succeed(result, "Get list order Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("users/{id}/orders")]
        public async Task<IActionResult> GetOrdersByUserId(int id)
        {
            try
            {
                var result = await _paymentService.GetOrdersByUserIdAsync(id);
                return Ok(ApiResult<List<OrderDTO>>.Succeed(result, "Get list order Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("users/me/orders")]
        public async Task<IActionResult> GetOrdersByCurrentUser()
        {
            try
            {
                if (_claimsService.GetCurrentUserId == -1)
                {
                    throw new Exception("401 - User have been not signed in");
                }

                var result = await _paymentService.GetOrdersByUserIdAsync(_claimsService.GetCurrentUserId);
                return Ok(ApiResult<List<OrderDTO>>.Succeed(result, "Get list order Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("orders")]
        public async Task<IActionResult> CheckOutAsync(VnpayOrderInfo orderInfo)
        {
            try
            {
                var result = await _paymentService.NewOrderAsync(orderInfo);
                if (result != null)
                {
                    return Ok(ApiResult<OrderDTO>.Succeed(result, "purchase successfully"));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("401"))
                    return Unauthorized(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("403"))
                    return StatusCode(StatusCodes.Status403Forbidden, ApiResult<object>.Fail(ex));

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("orders/{id}/cancelOrder")]
        public async Task<IActionResult> CancelOrderAsync(int id)
        {
            try
            {
                var result = await _paymentService.CancelOrderAsync(id);
                if (result != null)
                {
                    return Ok(ApiResult<OrderDTO>.Succeed(result, "Order Canceled"));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("401"))
                    return Unauthorized(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("403"))
                    return StatusCode(StatusCodes.Status403Forbidden, ApiResult<object>.Fail(ex));

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}