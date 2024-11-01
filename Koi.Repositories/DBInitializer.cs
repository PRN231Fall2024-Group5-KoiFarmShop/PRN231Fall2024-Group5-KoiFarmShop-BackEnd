using Koi.BusinessObjects;
using Koi.Repositories.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories
{
    public static class DBInitializer
    {
        public static async Task Initialize(
            KoiFarmShopDbContext context,
            UserManager<User> userManager)
        {
            #region Seed Roles (sử dụng UserManager cho AddToRoleAsync mà không cần RoleManager)

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

            #endregion Seed Roles (sử dụng UserManager cho AddToRoleAsync mà không cần RoleManager)

            #region Seed Users

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
                    ImageUrl = "https://example.com/manager.png",
                    Address = "Manager Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    RoleName = "ADMIN"
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
                    ImageUrl = "https://example.com/manager.png",
                    Address = "Manager Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    RoleName = "MANAGER"
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
                    ImageUrl = "https://example.com/staff.png",
                    Address = "Staff Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    RoleName = "STAFF"
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
                    ImageUrl = "https://example.com/customer.png",
                    Address = "Customer Street, City",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    RoleName = "CUSTOMER"
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
                        ImageUrl = "https://scontent.fsgn15-1.fna.fbcdn.net/v/t39.30808-1/430878538_2206677789683723_4464660377243750146_n.jpg",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7),
                        RoleName="STAFF"
                    },
                    new User
                    {
                        UserName = "namthhse172294",
                        Email = "namthhse172294@fpt.edu.vn",
                        FullName = "Trương Hà Hào Nam",
                        UnsignFullName = StringTools.ConvertToUnSign("Trương Hà Hào Nam"),
                        Dob = new DateTime(2003, 1, 1),
                        PhoneNumber = "0123456789",
                        ImageUrl = "https://avatar.iran.liara.run/public/boy?username=namthhse172294",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7),
                                            RoleName = "CUSTOMER"
                    },
                    new User
                    {
                        UserName = "vunse172437",
                        Email = "vunse172437@fpt.edu.vn",
                        FullName = "Nguyễn Vũ",
                        Dob = new DateTime(2003, 2, 15),
                        PhoneNumber = "0123456789",
                        ImageUrl = "https://avatar.iran.liara.run/public/boy?username=vunse172437",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7),
                                                                    RoleName = "CUSTOMER"
                    },
                    new User
                    {
                        UserName = "huanngse171018",
                        Email = "huanngse171018@fpt.edu.vn",
                        FullName = "Ngô Gia Huấn",
                        UnsignFullName = StringTools.ConvertToUnSign("Ngô Gia Huấn"),
                        Dob = new DateTime(2003, 3, 20),
                        PhoneNumber = "0123456789",
                        ImageUrl = "https://avatar.iran.liara.run/public/boy?username=huanngse171018",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7),
                                                                    RoleName = "CUSTOMER"
                    },
                    new User
                    {
                        UserName = "tienhmse172436",
                        Email = "tienhmse172436@fpt.edu.vn",
                        FullName = "Hoàng Minh Tiến Lmao",
                        UnsignFullName = StringTools.ConvertToUnSign("Hoàng Minh Tiến"),
                        Dob = new DateTime(2003, 4, 5),
                        PhoneNumber = "0123456789",
                        ImageUrl = "https://avatar.iran.liara.run/public/boy?username=tienhmse172436",
                        Address = "HCM",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(7),
                                                                    RoleName = "CUSTOMER"
                    }
                };

                // Sử dụng AddRange để thêm danh sách khách hàng
                foreach (var customerr in customers)
                {
                    await CreateUserAsync(userManager, customer, "123456", customer.RoleName);
                }
                await context.SaveChangesAsync();
            }

            var allUsers = await context.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                if (string.IsNullOrEmpty(user.SecurityStamp))
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    Console.WriteLine($"Security stamp updated for user {user.UserName}");
                }
            }

            #endregion Seed Users

            #region Seed KoiBreeds

            if (!context.KoiBreeds.Any())
            {
                List<KoiBreed> breeds = new(){
                    new KoiBreed
                    {
                        Name = "Kohaku",
                        Content = "One of the oldest and most iconic koi breeds, Kohaku originated in Japan centuries ago. The name \"Kohaku\" translates to \"red and white\". Red and white often symbolize good fortune and prosperity in Japanese culture. Red is associated with passion, energy, and joy, while white represents purity and peace. Kohaku are prized for their bold and striking color contrast. The arrangement of the red markings, often in a triangular pattern, is a key factor in determining the quality of a Kohaku."
                    },
                    new KoiBreed
                    {
                        Name = "Sanke",
                        Content = "Sanke is a relatively new breed, developed in the early 20th century. It's a combination of Kohaku and Showa. The addition of black markings to the Kohaku pattern adds a layer of complexity and sophistication. Black is often associated with strength, mystery, and wisdom. Sanke are known for their elegant and balanced appearance. The interplay between the red, white, and black markings creates a visually appealing and harmonious design."
                    },
                    new KoiBreed
                    {
                        Name = "Showa",
                        Content = "Showa was developed in the early 20th century and is named after the Showa era in Japan. The black base represents strength and stability, while the red and white markings add dynamism and energy. Showa are characterized by their bold and contrasting colors. The arrangement of the markings, often in a scattered or irregular pattern, is a key factor in determining their quality."
                    },
                    new KoiBreed
                    {
                        Name = "Utsurimono",
                        Content = "Utsurimono is a broad category of koi with a black base and various patterns. The different patterns (Shiro Utsuri, Hi Utsuri, Ki Utsuri) represent different levels of intensity and complexity. Utsurimono are known for their versatility and adaptability. The various patterns and color combinations offer a wide range of aesthetic options."
                    },
                    new KoiBreed
                    {
                        Name = "Asagi",
                        Content = "Asagi is believed to have originated from a natural mutation. The blue color is often associated with tranquility, harmony, and loyalty. Asagi are prized for their delicate and elegant appearance. The \"sashimono\" pattern, consisting of small, dark markings on the blue base, is a distinctive feature of this breed."
                    },
                    new KoiBreed
                    {
                        Name = "Tancho",
                        Content = "Tancho is a distinctive koi variety characterized by a prominent red or black \"spot\" on its head, often resembling a cherry blossom. This unique marking is believed to symbolize good fortune and prosperity in Japanese culture. The red or black spot can vary in size and shape, and its appearance can significantly impact the value of a Tancho koi. The contrast between the spot and the surrounding white or silver base creates a striking and visually appealing fish."
                    },
                    new KoiBreed
                    {
                        Name = "Goshiki",
                        Content = "Goshiki is a captivating koi breed that combines the characteristics of Asagi and Kohaku. It features a blue base with red and white markings, often arranged in a harmonious and balanced pattern. The interplay between the three colors creates a stunning and visually complex fish. Goshiki are highly prized by koi enthusiasts for their beauty and rarity. The quality of a Goshiki is determined by the intensity of the colors, the arrangement of the markings, and the overall balance of the fish."
                    },
                    new KoiBreed
                    {
                        Name = "Bekko",
                        Content = "Bekko is a classic koi breed known for its bold and contrasting colors. It features a black base with white or yellow markings, often arranged in a scattered or irregular pattern. The combination of dark and light colors creates a visually striking and dynamic fish. Bekko koi are available in various patterns, including Shiro Bekko (white markings on a black base), Ki Bekko (yellow markings on a black base), and Gin Bekko (silver markings on a black base). The quality of a Bekko is determined by the intensity of the colors, the arrangement of the markings, and the overall balance of the fish."
                    },
                    new KoiBreed
                    {
                        Name = "Yamabuki",
                        Content = "Yamabuki is a stunning koi breed characterized by its vibrant yellow color. The absence of any markings on the yellow base creates a pure and striking appearance. Yamabuki koi are highly sought after for their unique and eye-catching color. The quality of a Yamabuki is determined by the intensity and evenness of the yellow color, as well as the overall health and condition of the fish."
                    },
                    new KoiBreed
                    {
                        Name = "Doitsu",
                        Content = "Doitsu is a unique koi breed characterized by its distinctive mirror-like scale pattern. Unlike traditional koi, Doitsu have a single row of scales along their lateral line, while the rest of their body is covered in a smooth, scaleless skin. This unusual appearance creates a sleek and modern look. Doitsu koi are available in various colors and patterns, including black, white, red, and blue. The quality of a Doitsu is determined by the clarity and symmetry of the scale pattern, as well as the overall health and condition of the fish."
                    }
                };
                var defaultUser = context.Users.FirstOrDefault(user => user.UserName == "vunse172437");
                foreach (var item in breeds)
                {
                    item.CreatedAt = DateTime.Now;
                    item.ModifiedAt = DateTime.Now;
                    item.CreatedBy = defaultUser.Id;
                    item.ModifiedBy = defaultUser.Id;
                }
                await context.KoiBreeds.AddRangeAsync(breeds);
                await context.SaveChangesAsync();
            }

            if (!context.Diets.Any())
            {
                var diet1 = new Diet
                {
                    Name = "Diet 1 ",
                    DietCost = 1000,
                    Description = ""
                };

                await context.AddAsync(diet1);

                var diet2 = new Diet
                {
                    Name = "Diet 2",
                    DietCost = 1000,
                    Description = ""
                };
                await context.AddAsync(diet2);
                await context.SaveChangesAsync();
            }

            #endregion Seed KoiBreeds

            #region Seed KoiFish

            var koifishes = await context.KoiFishs.Include(x => x.ConsignmentForNurtures).ToListAsync();
            // Identify any KoiFish that are marked as consigned but do not have related consignments
            var errorFishes = koifishes.Where(x => x.IsConsigned == true && !x.ConsignmentForNurtures.Any()).ToList();
            if (errorFishes.Any())
            {
                foreach (var fish in errorFishes)
                {
                    fish.IsConsigned = false;
                }
                await context.SaveChangesAsync(); // Save any updates made to orphaned KoiFish
            }
            if (!koifishes.Any())
            {
                // Ensure KoiBreeds are loaded
                var koiBreeds = context.KoiBreeds.ToList();

                List<KoiFish> fishList = new()
                {
                    new KoiFish
                    {
                        Name = "Hikari",
                        Origin = "Japan",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 25,
                        Weight = 1500,
                        PersonalityTraits = "Playful, curious",
                        DailyFeedAmount = 100,
                        LastHealthCheck = DateTime.Now.AddDays(-30),
                        IsAvailableForSale = true,
                        Price = 500000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Kohaku"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Sanke")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Sakura",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 30,
                        Weight = 2000,
                        PersonalityTraits = "Gentle, shy",
                        DailyFeedAmount = 120,
                        LastHealthCheck = DateTime.Now.AddDays(-20),
                        IsAvailableForSale = false,
                        Price = 800000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Showa"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Asagi")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Ryu",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 22,
                        Weight = 1200,
                        PersonalityTraits = "Aggressive, active",
                        DailyFeedAmount = 90,
                        LastHealthCheck = DateTime.Now.AddDays(-45),
                        IsAvailableForSale = true,
                        Price = 600000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Utsurimono")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Kumo",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 35,
                        Weight = 2500,
                        PersonalityTraits = "Calm, friendly",
                        DailyFeedAmount = 150,
                        LastHealthCheck = DateTime.Now.AddDays(-10),
                        IsAvailableForSale = true,
                        Price = 700000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Tancho")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Aoi",
                        Origin = "Japan",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 20,
                        Weight = 800,
                        PersonalityTraits = "Energetic, playful",
                        DailyFeedAmount = 80,
                        LastHealthCheck = DateTime.Now.AddDays(-60),
                        IsAvailableForSale = false,
                        Price = 400000,
                        IsConsigned = false,
                        IsSold = true,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Goshiki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Mizu",
                        Origin = "China",
                        Gender = false,
                        Dob = new DateTime(2024,01,01),
                        Length = 28,
                        Weight = 1800,
                        PersonalityTraits = "Gentle, curious",
                        DailyFeedAmount = 110,
                        LastHealthCheck = DateTime.Now.AddDays(-15),
                        IsAvailableForSale = true,
                        Price = 650000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Bekko")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Taro",
                        Origin = "Japan",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 32,
                        Weight = 2200,
                        PersonalityTraits = "Strong, dominant",
                        DailyFeedAmount = 130,
                        LastHealthCheck = DateTime.Now.AddDays(-40),
                        IsAvailableForSale = true,
                        Price = 750000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Yamabuki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Kira",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 24,
                        Weight = 1400,
                        PersonalityTraits = "Active, friendly",
                        DailyFeedAmount = 100,
                        LastHealthCheck = DateTime.Now.AddDays(-25),
                        IsAvailableForSale = true,
                        Price = 550000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Doitsu")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Haruka",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 27,
                        Weight = 1600,
                        PersonalityTraits = "Playful, shy",
                        DailyFeedAmount = 110,
                        LastHealthCheck = DateTime.Now.AddDays(-35),
                        IsAvailableForSale = true,
                        Price = 620000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Kohaku"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Showa")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Nami",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 29,
                        Weight = 1700,
                        PersonalityTraits = "Calm, intelligent",
                        DailyFeedAmount = 120,
                        LastHealthCheck = DateTime.Now.AddDays(-50),
                        IsAvailableForSale = false,
                        Price = 680000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Goshiki"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Tancho")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Yuki",
                        Origin = "Japan",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 31,
                        Weight = 1900,
                        PersonalityTraits = "Energetic, friendly",
                        DailyFeedAmount = 140,
                        LastHealthCheck = DateTime.Now.AddDays(-5),
                        IsAvailableForSale = true,
                        Price = 720000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Utsurimono"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Doitsu")
                        }
                    },
                    // Additional KoiFish Entries
                    new KoiFish
                    {
                        Name = "Sora",
                        Origin = "Japan",
                        Gender = false,
                                Dob = new DateTime(2024, 01, 01),
                        Length = 26,
                        Weight = 1550,
                        PersonalityTraits = "Playful, curious",
                        DailyFeedAmount = 105,
                        LastHealthCheck = DateTime.Now.AddDays(-12),
                        IsAvailableForSale = true,
                        Price = 560000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Kohaku")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Akira",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 23,
                        Weight = 1300,
                        PersonalityTraits = "Aggressive, strong",
                        DailyFeedAmount = 95,
                        LastHealthCheck = DateTime.Now.AddDays(-55),
                        IsAvailableForSale = true,
                        Price = 620000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Sanke")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Momo",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 29,
                        Weight = 1800,
                        PersonalityTraits = "Gentle, intelligent",
                        DailyFeedAmount = 115,
                        LastHealthCheck = DateTime.Now.AddDays(-28),
                        IsAvailableForSale = false,
                        Price = 700000,
                        IsConsigned = false,
                        IsSold = true,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Showa")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Hana",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 24,
                        Weight = 1450,
                        PersonalityTraits = "Energetic, playful",
                        DailyFeedAmount = 105,
                        LastHealthCheck = DateTime.Now.AddDays(-22),
                        IsAvailableForSale = true,
                        Price = 650000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Asagi")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Kaze",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 30,
                        Weight = 2000,
                        PersonalityTraits = "Calm, friendly",
                        DailyFeedAmount = 120,
                        LastHealthCheck = DateTime.Now.AddDays(-10),
                        IsAvailableForSale = true,
                        Price = 700000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Tancho"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Bekko")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Riko",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 21,
                        Weight = 1200,
                        PersonalityTraits = "Playful, active",
                        DailyFeedAmount = 85,
                        LastHealthCheck = DateTime.Now.AddDays(-32),
                        IsAvailableForSale = true,
                        Price = 550000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Goshiki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Miki",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 28,
                        Weight = 1600,
                        PersonalityTraits = "Calm, gentle",
                        DailyFeedAmount = 110,
                        LastHealthCheck = DateTime.Now.AddDays(-15),
                        IsAvailableForSale = true,
                        Price = 680000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Doitsu")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Tsubasa",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 33,
                        Weight = 2200,
                        PersonalityTraits = "Strong, dominant",
                        DailyFeedAmount = 135,
                        LastHealthCheck = DateTime.Now.AddDays(-40),
                        IsAvailableForSale = true,
                        Price = 750000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Kohaku"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Showa")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Niko",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 26,
                        Weight = 1700,
                        PersonalityTraits = "Energetic, curious",
                        DailyFeedAmount = 125,
                        LastHealthCheck = DateTime.Now.AddDays(-22),
                        IsAvailableForSale = true,
                        Price = 690000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Bekko"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Utsurimono")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Sumi",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01) ,
                        Length = 23,
                        Weight = 1400,
                        PersonalityTraits = "Playful, shy",
                        DailyFeedAmount = 100,
                        LastHealthCheck = DateTime.Now.AddDays(-10),
                        IsAvailableForSale = true,
                        Price = 640000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Yamabuki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Aika",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024, 01, 01)                ,
                        Length = 20,
                        Weight = 1150,
                        PersonalityTraits = "Gentle, playful",
                        DailyFeedAmount = 90,
                        LastHealthCheck = DateTime.Now.AddDays(-60),
                        IsAvailableForSale = true,
                        Price = 530000,
                        IsConsigned = true,
                        IsSold = false,
                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Doitsu"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Goshiki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Kaito",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024, 01, 01),
                        Length = 30,
                        Weight = 2000,
                        PersonalityTraits = "Strong, dominant",
                        DailyFeedAmount = 125,
                        LastHealthCheck = DateTime.Now.AddDays(-20),
                        IsAvailableForSale = true,
                        Price = 720000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Tancho"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Showa")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Kokoro",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024,01,01),
                        Length = 27,
                        Weight = 1550,
                        PersonalityTraits = "Calm, friendly",
                        DailyFeedAmount = 105,
                        LastHealthCheck = DateTime.Now.AddDays(-25),
                        IsAvailableForSale = false,
                        Price = 670000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Kohaku"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Bekko")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Hoshi",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024,01,01),
                        Length = 25,
                        Weight = 1450,
                        PersonalityTraits = "Playful, energetic",
                        DailyFeedAmount = 100,
                        LastHealthCheck = DateTime.Now.AddDays(-40),
                        IsAvailableForSale = true,
                        Price = 610000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Sanke"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Utsurimono")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Mika",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024,01,01),
                        Length = 31,
                        Weight = 1750,
                        PersonalityTraits = "Gentle, shy",
                        DailyFeedAmount = 115,
                        LastHealthCheck = DateTime.Now.AddDays(-15),
                        IsAvailableForSale = true,
                        Price = 690000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Doitsu"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Goshiki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Tomo",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024,01,01),
                        Length = 29,
                        Weight = 1900,
                        PersonalityTraits = "Strong, dominant",
                        DailyFeedAmount = 130,
                        LastHealthCheck = DateTime.Now.AddDays(-30),
                        IsAvailableForSale = true,
                        Price = 710000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Kohaku"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Tancho")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Luna",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024,01,01),
                        Length = 22,
                        Weight = 1200,
                        PersonalityTraits = "Curious, playful",
                        DailyFeedAmount = 95,
                        LastHealthCheck = DateTime.Now.AddDays(-35),
                        IsAvailableForSale = true,
                        Price = 560000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Asagi")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Harumi",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024,01,01),
                        Length = 21,
                        Weight = 1100,
                        PersonalityTraits = "Energetic, active",
                        DailyFeedAmount = 85,
                        LastHealthCheck = DateTime.Now.AddDays(-20),
                        IsAvailableForSale = true,
                        Price = 550000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Showa")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Rin",
                        Origin = "Japan",
                        Gender = false,
                        Dob = new DateTime(2024,01,01),
                        Length = 28,
                        Weight = 1600,
                        PersonalityTraits = "Calm, gentle",
                        DailyFeedAmount = 110,
                        LastHealthCheck = DateTime.Now.AddDays(-25),
                        IsAvailableForSale = true,
                        Price = 670000,
                        IsConsigned = true,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Yamabuki")
                        }
                    },
                    new KoiFish
                    {
                        Name = "Ryo",
                        Origin = "China",
                        Gender = true,
                        Dob = new DateTime(2024,01,01),
                        Length = 32,
                        Weight = 2100,
                        PersonalityTraits = "Strong, dominant",
                        DailyFeedAmount = 135,
                        LastHealthCheck = DateTime.Now.AddDays(-15),
                        IsAvailableForSale = true,
                        Price = 740000,
                        IsConsigned = false,
                        IsSold = false,

                        KoiBreeds = new List<KoiBreed>
                        {
                            koiBreeds.FirstOrDefault(b => b.Name == "Goshiki"),
                            koiBreeds.FirstOrDefault(b => b.Name == "Tancho")
                        }
                    }
                };
                var defaultUser = await context.Users.FirstOrDefaultAsync(user => user.UserName == "vunse172437");
                foreach (var item in fishList)
                {
                    item.CreatedAt = DateTime.Now;
                    item.ModifiedAt = DateTime.Now;
                    item.CreatedBy = defaultUser.Id;
                    item.ModifiedBy = defaultUser.Id;
                }
                await context.KoiFishs.AddRangeAsync(fishList);
                await context.SaveChangesAsync();
            }

            #endregion Seed KoiFish
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