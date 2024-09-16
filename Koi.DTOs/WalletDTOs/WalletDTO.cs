using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.WalletDTOs
{
    public class WalletDTO
    {
        public int UserId { get; set; } // Primary key and foreign key pointing to User

        public Int64 Balance { get; set; }
        public int LoyaltyPoints { get; set; }
        public string Status { get; set; }
    }
}