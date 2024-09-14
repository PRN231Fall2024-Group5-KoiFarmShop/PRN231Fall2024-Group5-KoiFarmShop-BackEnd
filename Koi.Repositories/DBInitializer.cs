using Koi.BusinessObjects;
using Koi.Repositories.Utils;
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
                    new Role { Name = "ADMIN", NormalizedName = "ADMIN" },
                };

                await context.AddRangeAsync(roles);

                await context.SaveChangesAsync();
            }

            // Seed Users
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FullName = "Admin",
                    UnsignFullName = "Admin User",
                    Dob = new DateTime(1980, 1, 1),
                    PhoneNumber = "0123456789",
                    ProfilePictureUrl = "https://example.com/manager.png",
                    Address = "Manager Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(7)
                };
                await CreateUserAsync(userManager, admin, "123456", "ADMIN");

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
                    CreatedDate = DateTime.UtcNow.AddHours(7)
                };
                await CreateUserAsync(userManager, manager, "123456", "MANAGER");

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
                    CreatedDate = DateTime.UtcNow.AddHours(7)
                };
                await CreateUserAsync(userManager, staff, "123456", "STAFF");

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
                    CreatedDate = DateTime.UtcNow.AddHours(7)
                };
                await CreateUserAsync(userManager, customer, "123456", "CUSTOMER");

                var customers = new List<User>
                {
                    new User
                    {
                        UserName = "uydev",
                        Email = "lequocuy@gmail.com",
                        FullName = "Lê Quốc Uy",
                        UnsignFullName = "Le Quoc Uy",
                        Dob = new DateTime(2003, 7, 11),
                        PhoneNumber = "0123456789",
                        ProfilePictureUrl = "https://scontent.fsgn15-1.fna.fbcdn.net/v/t39.30808-1/430878538_2206677789683723_4464660377243750146_n.jpg",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7)
                    },
                    new User
                    {
                        UserName = "namthhse172294",
                        Email = "namthhse172294@fpt.edu.vn",
                        FullName = "Trương Hà Hào Nam",
                        UnsignFullName = StringTools.ConvertToUnSign("Trương Hà Hào Nam"),
                        Dob = new DateTime(2003, 1, 1),
                        PhoneNumber = "0123456789",
                        ProfilePictureUrl = "https://avatar.iran.liara.run/public/boy?username=namthhse172294",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7)
                    },
                    new User
                    {
                        UserName = "vunse172437",
                        Email = "vunse172437@fpt.edu.vn",
                        FullName = "Nguyễn Vũ",
                        Dob = new DateTime(2003, 2, 15),
                        PhoneNumber = "0123456789",
                        ProfilePictureUrl = "https://avatar.iran.liara.run/public/boy?username=vunse172437",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7)
                    },
                    new User
                    {
                        UserName = "huanngse171018",
                        Email = "huanngse171018@fpt.edu.vn",
                        FullName = "Ngô Gia Huấn",
                        UnsignFullName = StringTools.ConvertToUnSign("Ngô Gia Huấn"),
                        Dob = new DateTime(2003, 3, 20),
                        PhoneNumber = "0123456789",
                        ProfilePictureUrl = "https://avatar.iran.liara.run/public/boy?username=huanngse171018",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7)
                    },
                    new User
                    {
                        UserName = "tienhmse172436",
                        Email = "tienhmse172436@fpt.edu.vn",
                        FullName = "Hoàng Minh Tiến Lmao",
                        UnsignFullName = StringTools.ConvertToUnSign("Hoàng Minh Tiến"),
                        Dob = new DateTime(2003, 4, 5),
                        PhoneNumber = "0123456789",
                        ProfilePictureUrl = "https://avatar.iran.liara.run/public/boy?username=tienhmse172436",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7)
                    }
                };

                // Sử dụng AddRange để thêm danh sách khách hàng
                foreach (var customerr in customers)
                {
                    await context.Users.AddAsync(customerr);
                }
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