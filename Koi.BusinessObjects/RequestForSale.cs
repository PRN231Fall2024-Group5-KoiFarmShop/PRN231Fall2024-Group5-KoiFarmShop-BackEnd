using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class RequestForSale : BaseEntity
    {
        public int UserId { get; set; }  // Foreign key to User
        public int KoiFishId { get; set; }
        public Int64? PriceDealed { get; set; }
        public String RequestStatus { get; set; }
        public String? Note { get; set; }

        // navigation
        public virtual User User { get; set; }
        public virtual KoiFish KoiFish { get; set; }
    }
}