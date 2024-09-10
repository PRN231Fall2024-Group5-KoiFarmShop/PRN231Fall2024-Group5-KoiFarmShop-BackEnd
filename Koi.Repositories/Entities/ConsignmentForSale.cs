using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Entities
{
    public class ConsignmentForSale : BaseEntity
    {
        public int CustomerId { get; set; }  // Foreign key to User
        public int KoiFishId { get; set; }   // Foreign key to KoiFish
        public string? ConsignmentType { get; set; }  // Loại ký gửi
        public DateTime ConsignmentDate { get; set; }  // Ngày ký gửi
        public Int64 PriceDealed { get; set; }  // Giá đã thỏa thuận
        public bool InspectionRequired { get; set; }  // Yêu cầu kiểm tra không
        public DateTime? InspectionDate { get; set; }  // Ngày kiểm tra (có thể null)
        public string? ConsignmentStatus { get; set; }  // Trạng thái ký gửi
        public int StaffId { get; set; }  // Foreign key to User (người nhân viên thực hiện)

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }  // Người khách hàng

        [ForeignKey("StaffId")]
        public virtual User Staff { get; set; }  // Nhân viên phụ trách

        public virtual KoiFish KoiFish { get; set; }  // Thông tin KoiFish liên quan
    }
}