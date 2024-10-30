using Koi.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow.AddHours(7); //PRODUCT OR TICKET
        public long TotalAmount { get; set; }
        public string? OrderStatus { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; } = "VNPAY";
        public string? Note { get; set; }
        public string? UserName { get; set; }

        public virtual WalletTransactionDTO? WalletTransaction { get; set; }  // Thêm navigation property cho WalletTransaction
        public virtual List<OrderDetailDTO>? OrderDetails { get; set; }
    }
}