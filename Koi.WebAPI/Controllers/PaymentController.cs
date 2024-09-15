using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Koi.Services.Services.VnPayConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.VnPayConfig;
using System.Web;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderService _paymentService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IOrderService paymentService, IVnPayService vnPayService)
        {
            _paymentService = paymentService;
            _vnPayService = vnPayService;
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

        [HttpPost("orders/check-out")]
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

        /// <summary>
        /// [DONT'T TOUCH] VnPay IPN Receiver [FromQuery] VnpayResponseModel vnpayResponseModel
        /// </summary>
        [HttpGet("orders/vnpay-ipn-receive")]
        public async Task<IActionResult> PaymentReturn()
        {
            try
            {
                var requestNameValue = _vnPayService.GetFullResponseData(Request.Query);
                //var htmlString = string.Empty;
                //var requestNameValue = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());

                //IPNReponse iPNReponse = await _vnPayService.IPNReceiver(
                //    vnpayResponseModel.vnp_TmnCode,
                //    vnpayResponseModel.vnp_SecureHash,
                //    vnpayResponseModel.vnp_TxnRef,
                //    vnpayResponseModel.vnp_TransactionStatus,
                //    vnpayResponseModel.vnp_ResponseCode,
                //    vnpayResponseModel.vnp_TransactionNo,
                //    vnpayResponseModel.vnp_BankCode,
                //    vnpayResponseModel.vnp_Amount,
                //    vnpayResponseModel.vnp_PayDate,
                //    vnpayResponseModel.vnp_BankTranNo,
                //    vnpayResponseModel.vnp_CardType, requestNameValue);

                return Ok(requestNameValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}