using Koi.BusinessObjects;
using Microsoft.AspNetCore.Identity;

namespace Koi.Repositories
{
    public static class DBInitializer
    {
        public static async Task Initialize(KoiFarmShopDbContext context, UserManager<User> userManager)
        {
            // Seed Roles (sử dụng UserManager cho AddToRoleAsync mà không cần RoleManager)
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "MANAGER", NormalizedName = "MANAGER" },
                    new Role { Name = "STAFF", NormalizedName = "STAFF" },
                    new Role { Name = "CUSTOMER", NormalizedName = "CUSTOMER" },
                    new Role { Name = "GUEST", NormalizedName = "GUEST" }
                };

                foreach (var role in roles)
                {
                    await context.Roles.AddAsync(role);
                }

                await context.SaveChangesAsync();
            }

            // Seed Users
            if (!context.Users.Any())
            {
                var manager = new User
                {
                    UserName = "manager",
                    Email = "manager@koifarm.com",
                    FullName = "Manager User",
                    UnsignFullName = "Manager User",
                    Dob = new DateTime(1980, 1, 1),
                    PhoneNumber = "0123456789",
                    ProfilePictureUrl = "https://example.com/manager.png",
                    Address = "Manager Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await CreateUserAsync(userManager, manager, "Manager@123", "MANAGER");

                var staff = new User
                {
                    UserName = "staff",
                    Email = "staff@koifarm.com",
                    FullName = "Staff User",
                    UnsignFullName = "Staff User",
                    Dob = new DateTime(1990, 2, 15),
                    PhoneNumber = "0123456789",
                    ProfilePictureUrl = "https://example.com/staff.png",
                    Address = "Staff Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await CreateUserAsync(userManager, staff, "Staff@123", "STAFF");

                var customer = new User
                {
                    UserName = "customer",
                    Email = "customer@koifarm.com",
                    FullName = "Customer User",
                    UnsignFullName = "Customer User",
                    Dob = new DateTime(2000, 5, 10),
                    PhoneNumber = "0123456789",
                    ProfilePictureUrl = "https://example.com/customer.png",
                    Address = "Customer Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await CreateUserAsync(userManager, customer, "Customer@123", "CUSTOMER");

                var guest = new User
                {
                    UserName = "guest",
                    Email = "guest@koifarm.com",
                    FullName = "Guest User",
                    UnsignFullName = "Guest User",
                    Dob = new DateTime(2002, 8, 25),
                    PhoneNumber = "0123456789",
                    ProfilePictureUrl = "https://example.com/guest.png",
                    Address = "Guest Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                await CreateUserAsync(userManager, guest, "Guest@123", "GUEST");

                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateUserAsync(UserManager<User> userManager, User user, string password, string role)
        {
            var userExist = await userManager.FindByEmailAsync(user.Email);
            if (userExist == null)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}