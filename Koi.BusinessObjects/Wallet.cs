using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class Wallet
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; } // Primary key and foreign key pointing to User

        public Int64 Balance { get; set; }
        public int LoyaltyPoints { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public DateTime? ModifiedAt { get; set; }

        // Navigation property for 1-1 relation with User
        public virtual User User { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}