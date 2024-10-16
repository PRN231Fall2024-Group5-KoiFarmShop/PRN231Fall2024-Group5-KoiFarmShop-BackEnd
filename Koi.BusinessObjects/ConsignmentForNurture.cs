using System.ComponentModel.DataAnnotations.Schema;

namespace Koi.BusinessObjects
{
    public class ConsignmentForNurture : BaseEntity
    {
        public int CustomerId { get; set; }  // Foreign key to User
        public int KoiFishId { get; set; }   // Foreign key to KoiFish
        public int DietId { get; set; } // Foreign Key to Diet

        public DateTime ConsignmentDate { get; set; }  // Ngày ký gửi
        public DateTime StartDate { get; set; }  // Ngày bắt đầu chăm sóc
        public DateTime EndDate { get; set; }  // Ngày kết thúc chăm sóc

        public Int64? FoodCost { get; set; }
        public Int64? LaborCost { get; set; }
        public int? DailyFeedAmount { get; set; }

        public long? PriceByDay { get; set; }  // Giá thỏa thuận cho mỗi ngày chăm sóc
        public int? TotalDays { get; set; }

        public Int64? ProjectedCost { get; set; }
        public Int64? ActualCost { get; set; }

        public bool? InspectionRequired { get; set; }
        public DateTime InspectionDate { get; set; }  // Ngày kết thúc chăm sóc
        public string ConsignmentStatus { get; set; }
        public string? Note { get; set; }

        public int StaffId { get; set; }  // Foreign key to User (nhân viên thực hiện)

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }  // Người khách hàng

        [ForeignKey("StaffId")]
        public virtual User Staff { get; set; }  // Nhân viên phụ trách

        public virtual KoiFish KoiFish { get; set; }  // Thông tin KoiFish liên quan

        [ForeignKey("PackageCareId")]
        public virtual PackageCare PackageCare { get; set; }

        public virtual Diet Diet { get; set; }
    }
}