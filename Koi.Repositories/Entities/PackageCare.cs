using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Entities
{
    public class PackageCare : BaseEntity
    {
        public string Name { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public Int64? FoodCost { get; set; }
        public Int64? LaborCost { get; set; }
        public Int64? TotalCost { get; set; }
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<ConsignmentForNurture> ConsignmentForNurtures { get; set; }
    }
}