using Koi.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Models.UserModels
{
    public class UserUpdateModel
    {
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 11 digits.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must contain only digits.")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.ImageUrl)]

        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public RoleEnums? RoleName { get; set; }
    }
}