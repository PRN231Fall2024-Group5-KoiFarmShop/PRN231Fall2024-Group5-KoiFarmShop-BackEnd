using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.BusinessObjects
{
    public class KoiDiary : BaseEntity
    {
        public int KoiFishId { get; set; }
        public int? UserId { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
        public virtual KoiFish KoiFish { get; set; }
    }
}