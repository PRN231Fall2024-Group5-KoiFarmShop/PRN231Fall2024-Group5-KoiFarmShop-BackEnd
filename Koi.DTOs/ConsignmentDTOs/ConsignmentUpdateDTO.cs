using Koi.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.ConsignmentDTOs
{
    public class ConsignmentUpdateDTO
    {
        public int? DietId { get; set; } // Change Diet
        public int? StaffId { get; set; } // Assign consignment to staff

        public DateTime ConsignmentDate { get; set; }  // Ngày ký gửi
        public DateTime StartDate { get; set; }  // Ngày bắt đầu chăm sóc
        public DateTime EndDate { get; set; }  // Ngày kết thúc chăm sóc
        public string? Note { get; set; }

        public Int64? DietCost { get; set; } // DietCost của diet
        public Int64? LaborCost { get; set; }

        //Projected cost sẽ dc tính toán lại nếu start date và end date thay đổi
        public Int64? ActualCost { get; set; } // cập nhật giá thực tế

        public bool? InspectionRequired { get; set; } // cập nhật yêu cầu
        public DateTime? InspectionDate { get; set; }  // Ngày kết thúc chăm sóc
        public ConsignmentStatusEnums ConsignmentStatus { get; set; }
    }
}