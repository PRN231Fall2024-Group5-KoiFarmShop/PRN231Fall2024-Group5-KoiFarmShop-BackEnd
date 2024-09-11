using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class Transaction : BaseEntity
    {
        public int UserId { get; set; }      // Foreign key to User
        public int OrderId { get; set; }     // Foreign key to Order
        public string? PaymentMethod { get; set; }   // Phương thức thanh toán
        public long Amount { get; set; }    // Tổng số tiền giao dịch
        public DateTime TransactionDate { get; set; }  // Ngày thực hiện giao dịch
        public string TransactionStatus { get; set; }  // Trạng thái giao dịch

        // Navigation properties
        public virtual User User { get; set; }

        public virtual Order Order { get; set; }
    }
}