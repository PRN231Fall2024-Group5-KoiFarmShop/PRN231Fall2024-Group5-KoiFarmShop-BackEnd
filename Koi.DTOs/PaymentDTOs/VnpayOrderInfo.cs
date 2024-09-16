using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.PaymentDTOs
{
    public class VnpayOrderInfo
    {
        public int? CommonId { get; set; }

        [Required]
        public long Amount { get; set; }

        public string Description { get; set; } = "";
    }
}