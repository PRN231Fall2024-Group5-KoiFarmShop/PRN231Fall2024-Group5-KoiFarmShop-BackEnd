using AutoMapper;
using Koi.BusinessObjects;
using Koi.Repositories;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Repositories.Repositories;
using Koi.Services.Interface;
using Koi.Services.Mapper;
using Koi.Services.Services;
using Koi.Services.Services.VnPayConfig;
using Koi.WebAPI.MiddleWares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Koi.WebAPI.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // CONNECT TO DATABASE
            services.AddDbContext<KoiFarmShopDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //sign up for middleware
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddTransient<PerformanceTimeMiddleware>();
            services.AddScoped<UserStatusMiddleware>(); // sử dụng ClaimsIdentity nên dùng Addscoped theo request

            //others
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IVnPayService, VnPayService>();

            // add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IKoiFishRepository, KoiFishRepository>();
            services.AddScoped<IKoiBreedRepository, KoiBreedRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IKoiCertificateRepository, KoiCertificateRepository>();
            services.AddScoped<IKoiDiaryRepository, KoiDiaryRepository>();

            // add generic repositories
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<KoiBreed>, GenericRepository<KoiBreed>>();
            services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
            services.AddScoped<IGenericRepository<Transaction>, GenericRepository<Transaction>>();
            services.AddScoped<IGenericRepository<KoiCertificate>, GenericRepository<KoiCertificate>>();
            services.AddScoped<IGenericRepository<KoiDiary>, GenericRepository<KoiDiary>>();

            // add signInManager
            services.AddScoped<SignInManager<User>>();
            // add services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IKoiBreedService, KoiBreedService>();
            services.AddScoped<IKoiFishService, KoiFishService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IKoiCertificateService, KoiCertificcateService>();

            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}