namespace Koi.BusinessObjects
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow.AddHours(7); //PRODUCT OR TICKET
        public long TotalAmount { get; set; }
        public string? OrderStatus { get; set; }
        public string? ShippingAddress { get; set; }
        public bool? IsConsignmentIncluded { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingMethod { get; set; }
        public string? Note { get; set; }
        public long IncompleteAmount { get; set; } = 0;

        //navigation
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual WalletTransaction WalletTransaction { get; set; }  // Thêm navigation property cho WalletTransaction

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual User User { get; set; }
    }
}