using Koi.DTOs.TransactionDTOs;

namespace Koi.DTOs.WalletDTOs
{
    public class DepositResponseDTO
    {
        public WalletTransactionDTO? Transaction { get; set; }
        public string? PayUrl { get; set; }
    }

    public class VnpayResponseDTO
    {
        public string vnp_TmnCode { get; set; } = string.Empty;
        public string vnp_BankCode { get; set; } = string.Empty;
        public string vnp_BankTranNo { get; set; } = string.Empty;
        public string vnp_CardType { get; set; } = string.Empty;
        public string vnp_OrderInfo { get; set; } = string.Empty;
        public string vnp_TransactionNo { get; set; } = string.Empty;
        public string vnp_TransactionStatus { get; set; } = string.Empty;
        public string? vnp_TxnRef { get; set; }
        public string vnp_SecureHashType { get; set; } = string.Empty;
        public string vnp_SecureHash { get; set; } = string.Empty;
        public string vnp_Amount { get; set; }
        public string? vnp_ResponseCode { get; set; }
        public string vnp_PayDate { get; set; } = string.Empty;
    }
}