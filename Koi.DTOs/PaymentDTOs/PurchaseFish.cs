using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class PurchaseFish
    {
        public int FishId { get; set; }
        public bool IsNuture { get; set; } = false;
    }
}