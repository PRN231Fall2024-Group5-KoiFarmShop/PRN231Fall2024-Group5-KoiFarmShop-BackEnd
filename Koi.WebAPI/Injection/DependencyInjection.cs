﻿using AutoMapper;
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
            services.AddScoped<IPayOSService, PayOSService>();

            // add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IKoiFishRepository, KoiFishRepository>();
            services.AddScoped<IKoiBreedRepository, KoiBreedRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IKoiCertificateRepository, KoiCertificateRepository>();
            services.AddScoped<IKoiDiaryRepository, KoiDiaryRepository>();
            services.AddScoped<IKoiImageRepository, KoiImageRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IDietRepository, DietRepository>();
            services.AddScoped<IConsignmentForNurtureRepository, ConsignmentForNurtureRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IWithdrawnRequestRepository, WithdrawnRequestRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IRequestForSaleRepository, RequestForSaleRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();
            // add generic repositories
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<KoiFish>, GenericRepository<KoiFish>>();
            services.AddScoped<IGenericRepository<KoiBreed>, GenericRepository<KoiBreed>>();
            services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
            services.AddScoped<IGenericRepository<Transaction>, GenericRepository<Transaction>>();
            services.AddScoped<IGenericRepository<KoiCertificate>, GenericRepository<KoiCertificate>>();
            services.AddScoped<IGenericRepository<KoiDiary>, GenericRepository<KoiDiary>>();
            services.AddScoped<IGenericRepository<KoiFishImage>, GenericRepository<KoiFishImage>>();
            services.AddScoped<IGenericRepository<OrderDetail>, GenericRepository<OrderDetail>>();
            services.AddScoped<IGenericRepository<Diet>, GenericRepository<Diet>>();
            services.AddScoped<IGenericRepository<ConsignmentForNurture>, GenericRepository<ConsignmentForNurture>>();
            services.AddScoped<IGenericRepository<Notification>, GenericRepository<Notification>>();
            services.AddScoped<IGenericRepository<WithdrawnRequest>, GenericRepository<WithdrawnRequest>>();
            services.AddScoped<IGenericRepository<Blog>, GenericRepository<Blog>>();
            services.AddScoped<IGenericRepository<RequestForSale>, GenericRepository<RequestForSale>>();
            services.AddScoped<IGenericRepository<FAQ>, GenericRepository<FAQ>>();
            // add signInManager
            services.AddScoped<SignInManager<User>>();
            // add services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IKoiBreedService, KoiBreedService>();
            services.AddScoped<IKoiFishService, KoiFishService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IKoiDiaryService, KoiDiaryService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IKoiCertificateService, KoiCertificateService>();
            services.AddScoped<IOrderDetailServices, OrderDetailServices>();
            services.AddScoped<IDietService, DietService>();
            services.AddScoped<IConsignmentForNurtureService, ConsignmentForNurtureService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IWithdrawnRequestService, WithdrawnRequestService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IRequestForSaleService, RequestForSaleService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IFAQService, FAQService>();
            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}