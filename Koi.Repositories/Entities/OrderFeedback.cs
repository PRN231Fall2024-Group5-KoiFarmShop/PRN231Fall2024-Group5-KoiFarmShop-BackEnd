using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Entities
{
    public class OrderFeedback : BaseEntity
    {
        public int OrderId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }

        // Navigation property
        public virtual Order Order { get; set; }
    }
}