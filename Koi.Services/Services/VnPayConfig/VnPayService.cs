using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Interfaces;
using Koi.Repositories.Utils;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Text;

namespace Koi.Services.Services.VnPayConfig
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IUnitOfWork _unitOfWork;

        public VnPayService(IConfiguration configuration, IClaimsService claimsService, ICurrentTime currentTime, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _unitOfWork = unitOfWork;
        }

        public SortedList<string, string> requestData
        = new SortedList<string, string>(new VnPayCompare());

        public SortedList<string, string> responseData
            = new SortedList<string, string>(new VnPayCompare());

        public string CreateLink(VnpayOrderInfo orderInfo)
        {
            var vnp_ReturnUrl = _configuration["Vnpay:ReturnUrl"];
            var vnp_PaymentUrl = _configuration["Vnpay:PaymentUrl"];
            var vnp_TmnCode = _configuration["Vnpay:TmnCode"];
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
            var vnp_Version = _configuration["Vnpay:Version"];

            this.vnp_Version = vnp_Version;
            vnp_Amount = $"{(int)orderInfo.Amount * 100}";
            vnp_Command = "pay";
            vnp_CreateDate = _currentTime.GetCurrentTime().ToString("yyyyMMddHHmmss");
            vnp_CurrCode = "VND";
            vnp_IpAddr = _claimsService.IpAddress; // LẤY RA IP ADDRESS TRONG CLAIMS
            vnp_Locale = "en";
            vnp_OrderInfo = "Pay " + orderInfo.Amount; //+ " for appointment number: " + orderInfo.AppointmentId + ", payment: " + orderInfo.PaymentId;
            vnp_OrderType = "email:" + _claimsService.GetCurrentUserId;
            this.vnp_ReturnUrl = vnp_ReturnUrl;
            this.vnp_TmnCode = vnp_TmnCode;
            vnp_TxnRef = orderInfo.CommonId.ToString(); // common id is transaction Id
            vnp_ExpireDate = _currentTime.GetCurrentTime().AddMinutes(15).ToString("yyyyMMddHHmmss");
            vnp_BankCode = "VNBANK"; // sai

            // Add request data to SortedList
            BindRequestData();

            return GetLink(vnp_PaymentUrl, vnp_HashSecret);
        }

        public PaymentResponseModel GetFullResponseData(IQueryCollection collection)
        {
            var hashSecret = _configuration["Vnpay:HashSecret"];

            var vnPay = new VnPayLibrary();

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            var orderId = Convert.ToInt64(vnPay.GetResponseData("vnp_TxnRef")); //mã tham chiếu được gửi về VNPay lúc tạo url
            var amount = Convert.ToDecimal(vnPay.GetResponseData("vnp_Amount"));
            var orderInfo = vnPay.GetResponseData("vnp_OrderInfo"); // nơi để appointment ID

            var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
            var banKTranNo = vnPay.GetResponseData("vnp_BankTranNo");

            string vnPayDateString = vnPay.GetResponseData("vnp_PayDate");
            DateTime vnPayDate = DateTime.ParseExact(vnPayDateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var transactionStatus = vnPay.GetResponseData("vnp_TransactionStatus");

            var vnpSecureHash =
               collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về
            var checkSignature =
                vnPay.ValidateSignature(vnpSecureHash, hashSecret); //check Signature

            if (!checkSignature)
                return new PaymentResponseModel()
                {
                    Success = false
                };

            return new PaymentResponseModel()
            {
                Success = transactionStatus == "00" ? true : false,
                PaymentMethod = "VnPay",
                OrderDescription = orderInfo,
                OrderId = orderId.ToString(),
                TransactionNo = vnPayTranId.ToString(),
                TransactionToken = vnpSecureHash,
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode,
                BanKTranNo = banKTranNo,
                AmountOfMoney = decimal.Divide(amount, 100),
                PayDate = vnPayDate,
            };
        }

        private string GetLink(string baseUrl, string secretKey)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string result = baseUrl + "?" + data.ToString();
            var secureHash = HashHelper.HmacSHA512(secretKey, data.ToString().Remove(data.Length - 1, 1));
            return result += "vnp_SecureHash=" + secureHash;
        }

        public async Task<IPNReponse> IPNReceiver(string vnpTmnCode, string vnpSecureHash, string vnpTxnRef,
            string vnpTransactionStatus, string vnpResponseCode, string vnpTransactionNo, string vnpBankCode,
            string vnpAmount, string vnpPayDate, string vnpBankTranNo, string vnpCardType,
            NameValueCollection requestNameValue)
        {
            foreach (string s in requestNameValue)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    responseData.Add(s, requestNameValue[s]);
                }
            }

            IPNReponse iPNReponse = new IPNReponse
            {
                price = vnpAmount,
                message = "Transaction Invalid",
                status = TransactionStatusEnums.FAILED,
                transactionId = 0
            };

            // Get HashSecret from env
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];

            var checkSignature = ValidateSignature(vnpSecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                try
                {
                    var txnId = int.Parse(vnpTxnRef);
                    iPNReponse.transactionId = txnId;
                }
                catch (Exception ex)
                {
                    return iPNReponse;
                }
                var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(iPNReponse.transactionId);
                if (transaction == null)
                {
                    //payment not found
                    return iPNReponse;
                }
                if (transaction.TransactionStatus == TransactionStatusEnums.COMPLETED.ToString())
                {
                    //payment paid
                    iPNReponse.status = TransactionStatusEnums.COMPLETED;
                    iPNReponse.message = "This transaction has been paid!";
                    return iPNReponse;
                }
                if (transaction.TransactionStatus == TransactionStatusEnums.FAILED.ToString())
                {
                    //payment failed
                    iPNReponse.status = TransactionStatusEnums.FAILED;
                    iPNReponse.message = "This transaction has been cancelled! Please create new order!";
                    return iPNReponse;
                }

                switch (vnpTransactionStatus)
                {
                    case "01": //Giao dich chưa hoàn tất
                        iPNReponse.status = TransactionStatusEnums.FAILED;
                        if (vnpResponseCode == "24")
                        {
                            iPNReponse.message = "Khách hàng hủy giao dịch";
                            await UpdateErrorTransaction(transaction, "Khách hàng hủy giao dịch");
                        }
                        else
                        {
                            iPNReponse.message = "Error 99: Giao dịch bị lỗi khác";
                            await UpdateErrorTransaction(transaction, "Error 99: Giao dịch bị lỗi khác");
                        }
                        break;

                    case "02": //Giao dịch bị lỗi
                        iPNReponse.status = TransactionStatusEnums.FAILED;
                        if (vnpResponseCode == "10")
                        {
                            iPNReponse.message = "Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần";
                            await UpdateErrorTransaction(transaction, "Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần");
                        }
                        else if (vnpResponseCode == "24")
                        {
                            iPNReponse.message = "Khách hàng hủy giao dịch";
                            await UpdateErrorTransaction(transaction, "Khách hàng hủy giao dịch");
                        }
                        else if (vnpResponseCode == "51")
                        {
                            iPNReponse.message = "Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.";
                            await UpdateErrorTransaction(transaction, "Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.");
                        }
                        else
                        {
                            iPNReponse.message = "Error 99: Giao dịch bị lỗi khác";
                            await UpdateErrorTransaction(transaction, "Error 99: Giao dịch bị lỗi khác");
                        }
                        break;
                    case "00": //giao dịch thành công
                        await UpdateStatusTransaction(transaction);
                        iPNReponse.status = TransactionStatusEnums.COMPLETED;
                        iPNReponse.message = "Transaction has been paid successfully!";
                        break;
                    default:
                        iPNReponse.status = TransactionStatusEnums.FAILED;
                        iPNReponse.message = "Invalid signature!";
                        break;
                }
                return iPNReponse;
            }
            //Invalid signature
            iPNReponse.status = TransactionStatusEnums.FAILED;
            iPNReponse.message = "Invalid signature!";
            return iPNReponse;
        }

        private async Task UpdateStatusTransaction(WalletTransaction transaction)
        {
            transaction.TransactionStatus = TransactionStatusEnums.COMPLETED.ToString();
            //Plus money to user wallet
            var wallet = await _unitOfWork.WalletRepository.GetAllWalletByIdAsync(transaction.WalletId);
            wallet.Balance += transaction.Amount;
            await _unitOfWork.TransactionRepository.Update(transaction);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateErrorTransaction(WalletTransaction transaction, string note)
        {
            transaction.TransactionStatus = TransactionStatusEnums.FAILED.ToString();
            transaction.Note = note;
            await _unitOfWork.TransactionRepository.Update(transaction);
            await _unitOfWork.SaveChangeAsync();
        }

        private bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = HashHelper.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            StringBuilder data = new StringBuilder();
            if (responseData.ContainsKey("vnp_SecureHashType"))
            {
                responseData.Remove("vnp_SecureHashType");
            }
            if (responseData.ContainsKey("vnp_SecureHash"))
            {
                responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        private void BindRequestData()
        {
            if (vnp_Amount != null)
                requestData.Add("vnp_Amount", vnp_Amount.ToString() ?? string.Empty);
            if (vnp_Command != null)
                requestData.Add("vnp_Command", vnp_Command);
            if (vnp_CreateDate != null)
                requestData.Add("vnp_CreateDate", vnp_CreateDate);
            if (vnp_CurrCode != null)
                requestData.Add("vnp_CurrCode", vnp_CurrCode);
            if (vnp_BankCode != null)
                requestData.Add("vnp_BankCode", vnp_BankCode);
            if (vnp_IpAddr != null)
                requestData.Add("vnp_IpAddr", vnp_IpAddr);
            if (vnp_Locale != null)
                requestData.Add("vnp_Locale", vnp_Locale);
            if (vnp_OrderInfo != null)
                requestData.Add("vnp_OrderInfo", vnp_OrderInfo);
            if (vnp_OrderType != null)
                requestData.Add("vnp_OrderType", vnp_OrderType);
            if (vnp_ReturnUrl != null)
                requestData.Add("vnp_ReturnUrl", vnp_ReturnUrl);
            if (vnp_TmnCode != null)
                requestData.Add("vnp_TmnCode", vnp_TmnCode);
            if (vnp_ExpireDate != null)
                requestData.Add("vnp_ExpireDate", vnp_ExpireDate);
            if (vnp_TxnRef != null)
                requestData.Add("vnp_TxnRef", vnp_TxnRef);
            if (vnp_Version != null)
                requestData.Add("vnp_Version", vnp_Version);
        }

        public string? vnp_Amount { get; set; }
        public string? vnp_Command { get; set; }
        public string? vnp_CreateDate { get; set; }
        public string? vnp_CurrCode { get; set; }
        public string? vnp_BankCode { get; set; }
        public string? vnp_IpAddr { get; set; }
        public string? vnp_Locale { get; set; }
        public string? vnp_OrderInfo { get; set; }
        public string? vnp_OrderType { get; set; }
        public string? vnp_ReturnUrl { get; set; }
        public string? vnp_TmnCode { get; set; }
        public string? vnp_ExpireDate { get; set; }
        public string? vnp_TxnRef { get; set; }
        public string? vnp_Version { get; set; }
    }
}