using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class PurchaseDTO
    {
        public List<int> FishIds { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Note { get; set; }
    }
}