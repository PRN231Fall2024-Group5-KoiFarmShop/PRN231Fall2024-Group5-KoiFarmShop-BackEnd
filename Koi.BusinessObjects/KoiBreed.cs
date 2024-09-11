using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class KoiBreed : BaseEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }

        // Navigation property
        public virtual ICollection<KoiFishKoiBreed> KoiFishKoiBreeds { get; set; }
    }
}