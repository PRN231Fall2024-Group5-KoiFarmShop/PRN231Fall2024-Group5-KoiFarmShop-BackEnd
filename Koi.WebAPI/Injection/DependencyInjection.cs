using AutoMapper;
using Koi.Repositories.Commons;
using Koi.Repositories.Entities;
using Koi.Repositories.Interfaces;
using Koi.Repositories;
using Koi.Services;
using Koi.WebAPI.MiddleWares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Koi.Services.Mapper;
using Koi.Repositories.Repositories;
using Koi.Services.Interface;
using Koi.Services.Services;

namespace Koi.WebAPI.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // CONNECT TO DATABASE
            services.AddDbContext<KoiFarmShopDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalDB"));
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
            // add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            // add generic repositories
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();

            // add signInManager
            services.AddScoped<SignInManager<User>>();
            // add services
            services.AddScoped<IUserService, UserService>();

            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}