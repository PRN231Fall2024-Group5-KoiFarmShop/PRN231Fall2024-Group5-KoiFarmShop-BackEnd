using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.DietDTOs
{
    public class DietDTO : DietCreateDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}