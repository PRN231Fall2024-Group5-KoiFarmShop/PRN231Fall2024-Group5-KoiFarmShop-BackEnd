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
        [Required]
        public int OrderId { get; set; }

        [Required]
        public long Amount { get; set; }

        public string Description { get; set; } = "";
    }
}