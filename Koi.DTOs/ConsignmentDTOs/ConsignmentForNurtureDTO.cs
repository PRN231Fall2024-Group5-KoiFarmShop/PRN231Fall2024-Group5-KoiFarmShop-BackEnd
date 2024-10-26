using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.ConsignmentDTOs
{
    public class ConsignmentForNurtureDTO
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }  // Foreign key to User
        public int? KoiFishId { get; set; }   // Foreign key to KoiFish
        public int? DietId { get; set; } // Foreign Key to Diet
        public int? StaffId { get; set; }
        public DateTime ConsignmentDate { get; set; }  // Ngày ký gửi
        public DateTime StartDate { get; set; }  // Ngày bắt đầu chăm sóc
        public DateTime EndDate { get; set; }  // Ngày kết thúc chăm sóc
        public string? Note { get; set; }

        public Int64? DietCost { get; set; }
        public Int64? LaborCost { get; set; }
        public int? DailyFeedAmount { get; set; }

        //    public long? PriceByDay { get; set; }  // Giá thỏa thuận cho mỗi ngày chăm sóc
        public int? TotalDays { get; set; }

        public Int64? ProjectedCost { get; set; } // gia du tinh
        public Int64? ActualCost { get; set; } // gia thuc te

        public bool? InspectionRequired { get; set; }
        public DateTime? InspectionDate { get; set; }  // Ngày kết thúc chăm sóc
        public string ConsignmentStatus { get; set; } = string.Empty;
    }
}