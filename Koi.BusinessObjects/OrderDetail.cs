using System.ComponentModel.DataAnnotations.Schema;

namespace Koi.BusinessObjects
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public int KoiFishId { get; set; }

        public int? ConsignmentForNurtureId { get; set; }

        public long Price { get; set; }
        public long? ConsignmentCost { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        public int? StaffId { get; set; }  // Foreign key to User (nhân viên thực hiện)

        //navigation
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("StaffId")]
        public virtual User? Staff { get; set; }

        public virtual KoiFish KoiFish { get; set; }
        public virtual ICollection<OrderDetailFeedback> OrderDetailFeedbacks { get; set; }
        public virtual ConsignmentForNurture? ConsignmentForNurture { get; set; }
    }
}