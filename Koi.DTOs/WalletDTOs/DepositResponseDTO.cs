using Koi.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.WalletDTOs
{
    public class DepositResponseDTO
    {
        public OrderDTO? Order { get; set; }
        public string? PayUrl { get; set; }
    }
}