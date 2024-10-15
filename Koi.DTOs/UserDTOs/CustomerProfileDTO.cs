using System;
using System.ComponentModel.DataAnnotations;

namespace Koi.DTOs.UserDTOs
{
    public class CustomerProfileDTO
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
    }
}