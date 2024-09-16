using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IVnPayService _vnPayService;

        public WalletController(IWalletService walletService, IVnPayService vnPayService)
        {
            _walletService = walletService;
            _vnPayService = vnPayService;
        }

        [HttpPost("wallets/deposit")]
        public async Task<IActionResult> DepositAsync(long amount)
        {
            try
            {
                var result = await _walletService.Deposit(amount);
                if (result != null)
                {
                    return Ok(ApiResult<DepositResponseDTO>.Succeed(result, "Create deposit order successfully"));
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
        [HttpGet("payment/vnpay-ipn-receive")]
        public async Task<IActionResult> PaymentReturn()
        {
            try
            {
                var result = await _walletService.UpdateBalanceWallet(Request.Query);

                return Ok(ApiResult<TransactionDTO>.Succeed(result, "Deposit paid successfully"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Lấy transaction theo Id
        [HttpGet("wallets/orders/transaction/{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            try
            {
                var result = await _walletService.GetTransactionById(transactionId);
                if (result != null)
                {
                    return Ok(ApiResult<TransactionDTO>.Succeed(result, "Get transaction successfully"));
                }
                else
                {
                    return NotFound(ApiResult<object>.Error(null, "Not found"));
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Lấy danh sách transaction theo OrderId
        [HttpGet("wallets/orders/{orderId}/transactions")]
        public async Task<IActionResult> GetTransactionsByOrderId(int orderId)
        {
            try
            {
                var result = await _walletService.GetTransactionsByOrderId(orderId);
                return Ok(ApiResult<List<TransactionDTO>>.Succeed(result, "Get list successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        // Hàm xử lý lỗi chung
        private IActionResult HandleError(Exception ex)
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