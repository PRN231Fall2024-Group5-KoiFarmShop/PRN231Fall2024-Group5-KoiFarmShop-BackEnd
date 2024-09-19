using Koi.DTOs.PaymentDTOs;
using Koi.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.WalletDTOs
{
    public class DepositResponseDTO
    {
        public WalletTransactionDTO? Transaction { get; set; }
        public string? PayUrl { get; set; }
    }
}