using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class ConsignmentForNurture : BaseEntity
    {
        public int CustomerId { get; set; }  // Foreign key to User
        public int KoiFishId { get; set; }   // Foreign key to KoiFish
        public int PackageCareId { get; set; } // Foreign Key to PackageCare

        public DateTime ConsignmentDate { get; set; }  // Ngày ký gửi
        public DateTime StartDate { get; set; }  // Ngày bắt đầu chăm sóc
        public DateTime EndDate { get; set; }  // Ngày kết thúc chăm sóc
        public long PriceByDayDeale { get; set; }  // Giá thỏa thuận cho mỗi ngày chăm sóc
        public bool InspectionRequired { get; set; }  // Có cần kiểm tra không
        public DateTime? InspectionDate { get; set; }  // Ngày kiểm tra (có thể null)
        public string ConsignmentStatus { get; set; }  // Trạng thái của ký gửi
        public int StaffId { get; set; }  // Foreign key to User (nhân viên thực hiện)

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }  // Người khách hàng

        [ForeignKey("StaffId")]
        public virtual User Staff { get; set; }  // Nhân viên phụ trách

        public virtual KoiFish KoiFish { get; set; }  // Thông tin KoiFish liên quan

        [ForeignKey("PackageCareId")]
        public virtual PackageCare PackageCare { get; set; }
    }
}