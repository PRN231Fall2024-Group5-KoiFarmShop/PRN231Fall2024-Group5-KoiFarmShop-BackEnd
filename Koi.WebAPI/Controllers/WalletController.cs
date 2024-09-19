﻿using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.TransactionDTOs;
using Koi.DTOs.WalletDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
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

                return Ok(ApiResult<WalletTransactionDTO>.Succeed(result, "Deposit paid successfully"));
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

        // Lấy danh sách transaction theo OrderId
        [HttpGet("users/{id}/wallet")]
        public async Task<IActionResult> GetWalletByUserId(int id)
        {
            try
            {
                var result = await _walletService.GetWalletByUserId(id);
                return Ok(ApiResult<WalletDTO>.Succeed(result, "Get list successfully"));
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
        [HttpPost("wallets/orders/{orderId}/complete-pending")]
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
        [HttpPost("payment/event-orders/{orderId}")]
        public async Task<IActionResult> PurchaseOrder(Guid orderId)
        {
            try
            {
                //var userId = _claimsService.GetCurrentUserId;

                //if (userId == Guid.Empty)
                //{
                //    throw new Exception("User Id is invalid");
                //}
                //if (orderId == Guid.Empty)
                //{
                //    throw new Exception("OrderId is invalid");
                //}
                //var result = await _walletService.PurchaseOrder(orderId, userId);
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

                throw new Exception("Purchase Order Failed! Not Have a Type!");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
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