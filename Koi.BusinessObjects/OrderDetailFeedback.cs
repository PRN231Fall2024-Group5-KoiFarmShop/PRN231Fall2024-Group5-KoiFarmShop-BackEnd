using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class OrderDetailFeedback : BaseEntity
    {
        public int OrderDetailId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }

        // Navigation property
        public virtual OrderDetail Order { get; set; }
    }
}