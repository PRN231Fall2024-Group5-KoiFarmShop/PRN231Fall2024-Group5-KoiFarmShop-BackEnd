using AutoMapper;
using Koi.BusinessObjects;
using Koi.Repositories;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Repositories.Repositories;
using Koi.Services.Interface;
using Koi.Services.Mapper;
using Koi.Services.Services;
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

            // add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IKoiFishRepository, KoiFishRepository>();
            services.AddScoped<IKoiBreedRepository, KoiBreedRepository>();

            // add generic repositories
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<KoiBreed>, GenericRepository<KoiBreed>>();

            // add signInManager
            services.AddScoped<SignInManager<User>>();
            // add services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IKoiBreedService, KoiBreedService>();
            services.AddScoped<IKoiFishService, KoiFishService>();

            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}