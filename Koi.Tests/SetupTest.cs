using AutoMapper;
using Koi.Repositories;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Koi.Services.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Tests
{
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly KoiFarmShopDbContext _dbContext;

        //setup for repository
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
       
        //setup for service
        protected readonly Mock<IKoiBreedService> _koiBreedServiceMock;
        protected readonly Mock<IKoiFishService> _koiFishServiceMock;
        protected readonly Mock<IUserService> _userServiceMock;
        protected readonly Mock<IClaimsService> _claimsServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;
        
        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new MapperConfigProfile());
                });
            _mapperConfig = mappingConfig.CreateMapper();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            //service
            _currentTimeMock = new Mock<ICurrentTime>();
            _claimsServiceMock = new Mock<IClaimsService>();
            _koiBreedServiceMock = new Mock<IKoiBreedService>();
            _koiFishServiceMock = new Mock<IKoiFishService>();

            var options = new DbContextOptionsBuilder<KoiFarmShopDbContext>()
                .UseSqlServer("Server=localhost;Database=koi-farm-shop-db;Integrated Security=True;")
                .Options;
            _dbContext = new KoiFarmShopDbContext(options);

            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(0);
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
