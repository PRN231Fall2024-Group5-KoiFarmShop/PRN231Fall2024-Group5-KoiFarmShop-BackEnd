using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class PackageCare : BaseEntity
    {
        public string Name { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public long? FoodCost { get; set; }
        public long? LaborCost { get; set; }
        public long? TotalCost { get; set; }
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<ConsignmentForNurture> ConsignmentForNurtures { get; set; }
    }
}