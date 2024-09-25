using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Models.UserModels
{
    public class UserDTO
    {
        public string Email { get; set; } = null!;

        public string? FullName { get; set; }
        public string? UnsignFullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? RoleName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public int? LoyaltyPoints { get; set; } = 0;
    }
}