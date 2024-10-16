using Koi.DTOs.DietDTOs;
using Koi.DTOs.KoiFishDTOs;
using Koi.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.DTOs.ConsignmentDTOs
{
    public class ConsignmentForNurtureDetailDTO : ConsignmentForNurtureDTO
    {
        ////////////

        public virtual CustomerProfileDTO Staff { get; set; }  // Nhân viên phụ trách

        public virtual KoiFishResponseDTO KoiFish { get; set; }  // Thông tin KoiFish liên quan

        public virtual DietDTO Diet { get; set; }
    }
}