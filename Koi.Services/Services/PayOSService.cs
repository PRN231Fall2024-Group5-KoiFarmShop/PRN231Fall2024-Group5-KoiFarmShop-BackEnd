using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Koi.Services.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayOSService> _logger;
        private readonly PayOS _payOS;
        private readonly IConfiguration _configuration;

        public PayOSService(ILogger<PayOSService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            var ClientId = _configuration["PayOS:ClientId"];
            var ChecksumKey = _configuration["PayOS:ChecksumKey"];
            var ApiKey = _configuration["PayOS:ApiKey"];
            _payOS = new PayOS(ClientId, ApiKey, ChecksumKey);
        }

        public async Task<string> CreateLink(int depositMoney, Guid txnRef)
        {
            var domain = "https://koifarmshop.netlify.app/payment";

            var paymentLinkRequest = new PaymentData(
                orderCode: long.Parse(txnRef.ToString().Substring(5)),
                amount: depositMoney,
                description: "Nạp tiền: " + depositMoney,
                items: [new("Nạp tiền " + depositMoney, 1, depositMoney)],
                returnUrl: domain + "?success=true&transactionId=" + "GG" + "&amount=" + depositMoney,
                cancelUrl: domain + "?canceled=true&transactionId=" + "GG" + "&amount=" + depositMoney
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            return response.checkoutUrl;
        }

        public async Task<PayOSWebhookResponse> ReturnWebhook(PayOSWebhookRequest payOSWebhookRequest)
        {
            // Log the receipt of the webhook
            //Seriablize the object to log
            _logger.LogInformation(JsonConvert.SerializeObject(payOSWebhookRequest));
            _logger.LogInformation("Received webhook with Code: {Code}, Success: {Success}", payOSWebhookRequest.Code, payOSWebhookRequest.Success);

            var ChecksumKey = _configuration["PayOS:ChecksumKey"];

            // Validate the webhook signature
            if (!PayOSUtils.IsValidData(payOSWebhookRequest, payOSWebhookRequest.Signature, ChecksumKey))
            {
                _logger.LogWarning("Invalid webhook signature for OrderCode: {OrderCode}", payOSWebhookRequest.Data.OrderCode);
                return new PayOSWebhookResponse
                {
                    Success = false,
                    Note = "Invalid signature"
                };
            }

            // Log the validated data
            _logger.LogInformation("Valid webhook data: OrderCode: {OrderCode}, Amount: {Amount}, Status: {Code}",
                payOSWebhookRequest.Data.OrderCode,
                payOSWebhookRequest.Data.Amount,
                payOSWebhookRequest.Code);

            // Handle the webhook based on the transaction status
            switch (payOSWebhookRequest.Code)
            {
                case "00":
                    _logger.LogInformation("Payment successful for OrderCode: {OrderCode}", payOSWebhookRequest.Data.OrderCode);

                    // Example: Update the order in the system, mark it as paid
                    //var order = await _unitOfWork.WalletRepository.ConfirmTransaction(payOSWebhookRequest.Data.OrderCode);

                    return new PayOSWebhookResponse
                    {
                        Success = true,
                        Note = "Payment processed successfully"
                    };

                case "01":
                    _logger.LogError("Invalid parameters in the webhook for OrderCode: {OrderCode}", payOSWebhookRequest.Data.OrderCode);
                    return new PayOSWebhookResponse
                    {
                        Success = false,
                        Note = "Invalid parameters"
                    };

                default:
                    _logger.LogWarning("Unhandled webhook code: {Code} for OrderCode: {OrderCode}", payOSWebhookRequest.Code, payOSWebhookRequest.Data.OrderCode);
                    return new PayOSWebhookResponse
                    {
                        Success = false,
                        Note = "Unhandled code"
                    };
            }
        }
    }

    public static class PayOSUtils
    {
        public static bool IsValidData(PayOSWebhookRequest payOSWebhook, string transactionSignature, string ChecksumKey)
        {
            try
            {
                JObject jsonObject = JObject.Parse(payOSWebhook.Data.ToString().Replace("'", "\""));
                var sortedKeys = jsonObject.Properties().Select(p => p.Name).OrderBy(k => k).ToList();

                StringBuilder transactionStr = new StringBuilder();
                foreach (var key in sortedKeys)
                {
                    string value = jsonObject[key]?.ToString() ?? string.Empty;
                    transactionStr.Append($"{key}={value}");
                    if (key != sortedKeys.Last())
                    {
                        transactionStr.Append("&");
                    }
                }

                string signature = ComputeHmacSha256(transactionStr.ToString(), ChecksumKey);
                return signature.Equals(transactionSignature, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public static string ComputeHmacSha256(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
    public class PayOSWebhookResponse
    {
        public bool Success { get; set; }
        public PayOSData Data { get; set; }
        public string Note { get; set; }
    }
    public class PayOSWebhookRequest
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public bool Success { get; set; }
        public PayOSData Data { get; set; }
        public string Signature { get; set; }
    }

    public class PayOSData
    {
        public int OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AccountNumber { get; set; }
        public string Reference { get; set; }
        public string TransactionDateTime { get; set; }
        public string Currency { get; set; }
        public string PaymentLinkId { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string CounterAccountBankId { get; set; }
        public string CounterAccountBankName { get; set; }
        public string CounterAccountName { get; set; }
        public string CounterAccountNumber { get; set; }
        public string VirtualAccountName { get; set; }
        public string VirtualAccountNumber { get; set; }
    }
}
