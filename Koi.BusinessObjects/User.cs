using Microsoft.AspNetCore.Identity;

namespace Koi.BusinessObjects
{
    public class User : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public string? UnsignFullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public string? RoleName { get; set; }
        public int? LoyaltyPoints { get; set; } = 0;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? DeleteBy { get; set; }
        public bool? IsDeleted { get; set; } = false;

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<KoiFish> KoiFishsConsigned { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual Wallet Wallet { get; set; }

        // Navigation directly to roles (many-to-many)
        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<WalletTransaction>? WalletTransactions { get; set; }  // Thêm navigation property cho WalletTransaction
    }
}