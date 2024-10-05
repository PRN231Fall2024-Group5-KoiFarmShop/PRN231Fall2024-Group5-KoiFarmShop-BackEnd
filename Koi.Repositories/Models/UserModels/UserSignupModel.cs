using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koi.DTOs.Enums;

namespace Koi.Repositories.Models.UserModels
{
    public class UserSignupModel
    {
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 Character")]
        [PasswordPropertyText]
        public string Password { get; set; } = "";

        //[Required(ErrorMessage = "Confirm Password is required!")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Password and confirmation does not match!")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 Character")]
        //[PasswordPropertyText]
        //public string ConfirmPassword { get; set; } = "";
        [Required(ErrorMessage = "Full Name is required!")]
        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone number must 10 digits")]
        public string? PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }
        public string? Address { get; set; }

        // public RoleEnums? RoleName { get; set; }
    }
}