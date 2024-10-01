using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class WalletTransaction : BaseEntity
    {
        //public int UserId { get; set; }  // Foreign key to User
        public int WalletId { get; set; }

        public int? OrderId { get; set; }  // Foreign key to Order (nullable vì  không phải mọi giao dịch đều liên quan đến đơn hàng)
        public string TransactionType { get; set; }  // Loại giao dịch (Deposit, Withdraw, Purchase, etc.)
        public string PaymentMethod { get; set; }  // Phương thức thanh toán (Ví dụ: Credit Card, Bank Transfer)
        public long Amount { get; set; }  // Số tiền giao dịch
        public long BalanceBefore { get; set; }  // Số dư trước khi giao dịch
        public long BalanceAfter { get; set; }  // Số dư sau khi giao dịch
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow.AddHours(7);  // Thời gian giao dịch
        public string TransactionStatus { get; set; }  // Trạng thái giao dịch (Success, Pending, Failed)
        public string? Note { get; set; }  // Ghi chú thêm (có thể null)

        // Navigation properties
        //public virtual User User { get; set; }  // Người dùng thực hiện giao dịch
        public virtual Order? Order { get; set; }  // Đơn hàng liên quan (có thể null)

        public virtual Wallet? Wallet { get; set; }  // Đơn hàng liên quan (có thể null)
    }
}