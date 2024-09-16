using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class PurchaseDTO
    {
        public int? FishId { get; set; }
        public Int64? Price { get; set; }
    }
}