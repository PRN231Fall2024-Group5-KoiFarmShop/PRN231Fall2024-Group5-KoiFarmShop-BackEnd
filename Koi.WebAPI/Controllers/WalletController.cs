using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.TransactionDTOs;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Koi.Services.Services;
using Koi.Services.Services.VnPayConfig;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using System.Reflection;
using System.Web;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IVnPayService _vnPayService;
        private readonly IClaimsService _claimsService;
        private readonly IPayOSService _payOSService;
        private readonly INotificationService _notificationService;

        public WalletController(IWalletService walletService, IVnPayService vnPayService, IClaimsService claimsService, IPayOSService payOSService, INotificationService notificationService)
        {
            _walletService = walletService;
            _vnPayService = vnPayService;
            _claimsService = claimsService;
            _payOSService = payOSService;
            _notificationService = notificationService;
        }

        [HttpPost("wallets/deposit")]
        public async Task<IActionResult> DepositAsync(long amount)
        {
            try
            {
                var result = await _walletService.Deposit(amount);
                if (result != null)
                {
                    // Create and push notification
                    var notification = new Notification
                    {
                        Title = "Deposit Successful",
                        Body = $"Your deposit of {amount} has been processed successfully.",
                        ReceiverId = _claimsService.GetCurrentUserId,
                        Type = "USER",
                        Url = "/profile/wallet",
                    };
                    await _notificationService.PushNotification(notification);

                    return Ok(ApiResult<DepositResponseDTO>.Succeed(result, "Create deposit order successfully"));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// [DONT'T TOUCH] VnPay IPN Receiver [FromQuery] VnpayResponseModel vnpayResponseModel
        /// </summary>
        [HttpGet("payment/vnpay-ipn-receive")]
        public async Task<IActionResult> PaymentReturn([FromQuery] VnpayResponseDTO vnpayResponseModel)
        {
            try
            {
                var htmlString = string.Empty;
                var requestNameValue = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());

                IPNReponse iPNReponse = await _vnPayService.IPNReceiver(
                    vnpayResponseModel.vnp_TmnCode,
                    vnpayResponseModel.vnp_SecureHash,
                    vnpayResponseModel.vnp_TxnRef,
                    vnpayResponseModel.vnp_TransactionStatus,
                    vnpayResponseModel.vnp_ResponseCode,
                    vnpayResponseModel.vnp_TransactionNo,
                    vnpayResponseModel.vnp_BankCode,
                    vnpayResponseModel.vnp_Amount,
                    vnpayResponseModel.vnp_PayDate,
                    vnpayResponseModel.vnp_BankTranNo,
                    vnpayResponseModel.vnp_CardType, requestNameValue);


                //Get root path and read index.html
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "index.html");

                using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        htmlString = sr.ReadToEnd();
                    }
                }
                string orderInfo = vnpayResponseModel.vnp_OrderInfo ?? "Không có thông tin";
                //format html
                var isSuccess = iPNReponse.status.ToString() == TransactionStatusEnums.COMPLETED.ToString();
                var textColor = isSuccess ? "text-green-500 dark:text-green-300" : "text-red-500 dark:text-red-300";
                var statusHTML = $"<p class=\"mt-1 text-md {textColor}\">{iPNReponse.status.ToString()}</p>";

                // Send notification
                //var notification = new Notification
                //{
                //    Title = "Deposit " + int.Parse(iPNReponse.price) / 100,
                //    Body = iPNReponse.message,
                //    UserId = _claimsService.GetCurrentUserId == Guid.Empty ? Guid.Empty : _claimsService.GetCurrentUserId,
                //    Url = "/profile/wallet",
                //    Sender = "System"
                //};
                //await _notificationService.PushNotification(notification).ConfigureAwait(true);

                //format image
                var imageHTML = string.Empty;
                if (isSuccess)
                {
                    imageHTML = $"<!--green: from-[#00b894] to-[#55efc4] -->\r\n                <!-- red: from-[#FF4B4B] to-[#FF8B8B] -->\r\n                <div class=\"absolute inset-0 bg-gradient-to-br from-[#00b894] to-[#55efc4] rounded-lg shadow-lg\">\r\n                    <div class=\"flex flex-col items-center justify-center h-full text-white\">\r\n                        <div class=\"text-6xl font-bold star\">✨</div>\r\n                        <!-- <div className=\"text-6xl font-bold hidden\">❌</div> -->\r\n                        <div class=\"wrapper\">\r\n                            <h1 class=\"mt-4 text-2xl font-bold\">Payment Successful</h1>\r\n                        </div>\r\n                    </div>\r\n                </div>";
                }
                else
                {
                    imageHTML = "<!--green: from-[#00b894] to-[#55efc4] -->\r\n                <!-- red: from-[#FF4B4B] to-[#FF8B8B] -->\r\n                <div class=\"absolute inset-0 bg-gradient-to-br from-[#FF4B4B] to-[#FF8B8B] rounded-lg shadow-lg\">\r\n                    <div class=\"flex flex-col items-center justify-center h-full text-white\">\r\n                        \r\n                        <div className=\"text-6xl font-bold hidden\">❌</div>\r\n                        <div class=\"wrapper\">\r\n                            <h1 class=\"mt-4 text-2xl font-bold\">Payment Failed</h1>\r\n                        </div>\r\n                    </div>\r\n                </div>>";
                }

                string htmlFormat = string.Format(htmlString, imageHTML, iPNReponse.transactionId.ToString(), $"{int.Parse(iPNReponse.price) / 100}", statusHTML, iPNReponse.message);

                return Content(htmlFormat, "text/html");
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
        [HttpGet("wallets/wallet-transactions/{transactionId}")]
        public async Task<IActionResult> GetWalletTransactionById(int transactionId)
        {
            try
            {
                var result = await _walletService.GetWalletTransactionById(transactionId);
                if (result != null)
                {
                    return Ok(ApiResult<WalletTransactionDTO>.Succeed(result, "Get transaction successfully"));
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

        /// <summary>
        /// Lấy danh sách transaction theo OrderId
        /// </summary>
        /// <returns>
        ///      list wallet transaction
        /// </returns>
        // Lấy danh sách transaction theo OrderId
        [HttpGet("wallets/orders/{orderId}/wallet-transactions")]
        public async Task<IActionResult> GetTransactionsByOrderId(int orderId)
        {
            try
            {
                var result = await _walletService.GetTransactionsByOrderId(orderId);
                return Ok(ApiResult<List<WalletTransactionDTO>>.Succeed(result, "Get list wallet transaction successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử tương tác với wallet
        /// </summary>
        /// <returns>
        ///      list wallet transaction
        /// </returns>
        // Lấy danh sách transaction theo OrderId
        [HttpGet("users/{id}/wallet-transactions")]
        public async Task<IActionResult> GetTransactionsByUserId(int id)
        {
            try
            {
                var result = await _walletService.GetWalletTransactionsByUserId(id);
                return Ok(ApiResult<List<WalletTransactionDTO>>.Succeed(result, "Get list wallet transaction successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử tương tác với wallet người dùng login
        /// </summary>
        /// <returns>
        ///      list wallet transaction
        /// </returns>
        // Lấy danh sách transaction theo OrderId
        [HttpGet("users/me/wallet-transactions")]
        public async Task<IActionResult> GetTransactionsByUserId()
        {
            try
            {
                if (_claimsService.GetCurrentUserId == -1)
                {
                    throw new Exception("401 - User have been not signed in");
                }

                var result = await _walletService.GetWalletTransactionsByUserId(_claimsService.GetCurrentUserId);
                return Ok(ApiResult<List<WalletTransactionDTO>>.Succeed(result, "Get list wallet transaction successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Get  wallet of a user
        /// </summary>
        /// <returns>
        ///      user wallet
        /// </returns>
        [HttpGet("users/{id}/wallets")]
        public async Task<IActionResult> GetWalletByUserId(int id)
        {
            try
            {
                var result = await _walletService.GetWalletByUserId(id);
                return Ok(ApiResult<WalletDTO>.Succeed(result, "Get wallet successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Get  wallet of current user
        /// </summary>
        /// <returns>
        ///      user wallet
        /// </returns>
        [HttpGet("users/me/wallets")]
        public async Task<IActionResult> GetWalletByUserId()
        {
            try
            {
                if (_claimsService.GetCurrentUserId == -1)
                {
                    throw new Exception("401 - User have been not signed in");
                }
                var result = await _walletService.GetWalletByUserId(_claimsService.GetCurrentUserId);
                return Ok(ApiResult<WalletDTO>.Succeed(result, "Get wallet successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Create direct url for old pending transaction
        /// </summary>
        /// <returns>
        ///     URL of payment
        /// </returns>
        [HttpPost("wallets/wallet-transactions/{transactionId}/complete-pending")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SubmitPendingOrderToPay(int transactionId)
        {
            try
            {
                var result = await _walletService.CompletePending(transactionId);

                if (result == null)
                {
                    throw new Exception("404 - Order not found");
                }
                else
                {
                    return Ok(ApiResult<DepositResponseDTO>.Succeed(result, "Payment to pay!"));
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Purchase a order
        /// </summary>
        [HttpPost("payment/purchase")]
        public async Task<IActionResult> PurchaseItems(PurchaseDTO purchaseDTO)
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId;

                if (userId == -1)
                {
                    throw new Exception("401 - User Id is invalid or not signed in");
                }

                var result = await _walletService.CheckOut(purchaseDTO);

                var notification = new Notification
                {
                    Title = "Purchase Successful",
                    Body = "Your purchase has been completed. Please check your wallet balance.",
                    ReceiverId = userId,
                    Type = "USER",
                    Url = "/profile/orders",
                };
                await _notificationService.PushNotification(notification);

                return Ok(ApiResult<OrderDTO>.Succeed(result, "Purchase successfully, please check your wallet!"));

                //if (result.Status == TransactionStatusEnums.FAILED.ToString())
                //{
                //    throw new Exception("Purchase Order Failed!");
                //}

                //if (result.Status == TransactionStatusEnums.PENDING.ToString())
                //{
                //    throw new Exception("Purchase Order Pending!");
                //}

                //if (result.Status == TransactionStatusEnums.SUCCESS.ToString())
                //{
                //    return Ok(ApiResult<TransactionResponsesDTO>.Succeed(result, "Purchase Order Successfully!"));
                //}
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Create Payment URL For PayOs
        /// </summary>
        [Route("create-payment-link-payos")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepositRequest depositRequest)
        {
            try
            {
                var result = await _walletService.DepositByPayOS(depositRequest.Amount);
                if (result != null)
                {
                    return Ok(ApiResult<DepositResponseDTO>.Succeed(result, "Create deposit with payos order successfully"));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("hook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookType webhookBody)
        {
            try
            {
                var result = await _payOSService.ReturnWebhook(webhookBody);

                if (result.Success)
                {
                    return Ok(new { Message = "Webhook processed successfully" });
                }

                return BadRequest(new { Message = "Webhook processing failed." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
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