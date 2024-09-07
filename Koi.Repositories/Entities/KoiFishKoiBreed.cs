using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Entities
{
    public class KoiFishKoiBreed
    {
        public int KoiFishId { get; set; } // Foreign key
        public virtual KoiFish KoiFish { get; set; } // Navigation property

        public int KoiBreedId { get; set; } // Foreign key
        public virtual KoiBreed KoiBreed { get; set; } // Navigation property
    }
}