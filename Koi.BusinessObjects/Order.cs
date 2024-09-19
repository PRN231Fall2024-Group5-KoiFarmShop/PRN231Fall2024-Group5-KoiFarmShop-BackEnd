namespace Koi.BusinessObjects
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow.AddHours(7); //PRODUCT OR TICKET
        public long TotalAmount { get; set; }
        public string? OrderStatus { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Note { get; set; }

        //navigation
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<WalletTransaction>? WalletTransactions { get; set; }  // Thêm navigation property cho WalletTransaction

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual User User { get; set; }
    }
}