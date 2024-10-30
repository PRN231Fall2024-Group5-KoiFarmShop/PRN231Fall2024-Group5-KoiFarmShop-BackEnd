using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.KoiFishDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int KoiFishId { get; set; }

        public int? ConsignmentForNurtureId { get; set; }

        public long Price { get; set; }
        public string? Status { get; set; }
        public string? ShippingStatus { get; set; }
        public string? NurtureStatus { get; set; }
        public int? StaffId { get; set; }
        public virtual KoiFishResponseDTO KoiFish { get; set; }
        public virtual ConsignmentForNurtureDTO? ConsignmentForNurture { get; set; }
    }
}