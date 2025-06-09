using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiBreedDTOs;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.DTOs.KoiDiaryDTOs;
using Koi.DTOs.KoiFishDTOs;
using Koi.DTOs.UserDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Services;
using Koi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Tests.Fish
{
    public class KoiFishServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IKoiFishService _service;
        private readonly Mock<IClaimsService> _claimsServiceMock;
        private readonly Mock<IKoiFishRepository> _fishRepositoryMock;
        private readonly Mock<IKoiBreedRepository> _breedRepositoryMock;

        public KoiFishServiceTests()
        {
            _fishRepositoryMock = new Mock<IKoiFishRepository>();
            _breedRepositoryMock = new Mock<IKoiBreedRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.KoiFishRepository).Returns(_fishRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.KoiBreedRepository).Returns(_breedRepositoryMock.Object);
            _claimsServiceMock = new Mock<IClaimsService>();
            _mapperMock = new Mock<IMapper>();
            _service = new KoiFishService(_unitOfWorkMock.Object, _mapperMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task GetKoiFishById_ReturnsKoiFishResponseDTO_WhenKoiFishExists()
        {
            // Arrange
            var koiFish = new KoiFish
            {
                Id = 1,
                Name = "Sakura",
                Origin = "Japan",
                Gender = true,
                Dob = DateTime.Now.AddYears(-2),
                Length = 60,
                Weight = 5000,
                Price = 1500000,
                Owner = new User { Id = 101, FullName = "John Doe" },  // Assuming User entity
                KoiCertificates = new List<KoiCertificate>
        {
            new KoiCertificate { Id = 1, KoiFishId = 1, CertificateType = "Breed Certificate", CertificateUrl = "http://example.com/cert1" }
        },
                KoiBreeds = new List<KoiBreed>
        {
            new KoiBreed { Id = 1, Name = "Showa" }
        },
                KoiFishImages = new List<KoiFishImage>
        {
            new KoiFishImage { Id = 1, ImageUrl = "http://example.com/koi1.jpg" }
        },
                KoiDiaries = new List<KoiDiary>
        {
            new KoiDiary { Id = 1, KoiFishId = 1, Description = "First health check", Date = DateTime.Now.AddMonths(-3) }
        }
            };

            var expectedResponse = new KoiFishResponseDTO
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Origin = koiFish.Origin,
                Gender = koiFish.Gender == true ? "Male" : "Female", // Assuming mapping gender to string
                Dob = koiFish.Dob,
                Length = koiFish.Length,
                Weight = koiFish.Weight,
                Price = koiFish.Price,
                Owner = new CustomerProfileDTO
                {
                    FullName = koiFish.Owner.FullName,
                    PhoneNumber = "123456789", // Assuming this is populated in actual mapping
                    Address = "Some Address"
                },
                KoiCertificates = koiFish.KoiCertificates.Select(cert => new KoiCertificateResponseDTO
                {
                    Id = cert.Id,
                    KoiFishId = cert.KoiFishId,
                    CertificateType = cert.CertificateType,
                    CertificateUrl = cert.CertificateUrl
                }).ToList(),
                KoiBreeds = koiFish.KoiBreeds.Select(breed => new KoiBreedResponseDTO
                {
                    Id = breed.Id,
                    Name = breed.Name
                }).ToList(),
                KoiFishImages = koiFish.KoiFishImages.Select(image => new KoiFishImageDTO
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl
                }).ToList(),
                KoiDiaries = koiFish.KoiDiaries.Select(diary => new KoiFishDiaryCreateDTO
                {
                    KoiFishId = diary.KoiFishId,
                    Description = diary.Description,
                    Date = diary.Date
                }).ToList()
            };

            _unitOfWorkMock.Setup(uow => uow.KoiFishRepository.GetByIdAsync(1,
                It.IsAny<Expression<Func<KoiFish, object>>[]>())).ReturnsAsync(koiFish);

            _mapperMock.Setup(m => m.Map<KoiFishResponseDTO>(koiFish)).Returns(expectedResponse);

            // Act
            var result = await _service.GetKoiFishById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            Assert.Equal(expectedResponse.Name, result.Name);
            Assert.Equal(expectedResponse.Owner.FullName, result.Owner.FullName);
            Assert.Equal(expectedResponse.KoiCertificates.Count, result.KoiCertificates.Count);
            Assert.Equal(expectedResponse.KoiBreeds.Count, result.KoiBreeds.Count);
            Assert.Equal(expectedResponse.KoiFishImages.Count, result.KoiFishImages.Count);
            Assert.Equal(expectedResponse.KoiDiaries.Count, result.KoiDiaries.Count);

            _unitOfWorkMock.Verify(uow => uow.KoiFishRepository.GetByIdAsync(1, It.IsAny<Expression<Func<KoiFish, object>>[]>()), Times.Once);
            _mapperMock.Verify(m => m.Map<KoiFishResponseDTO>(It.IsAny<KoiFish>()), Times.Once);
        }


        [Fact]
        public async Task GetKoiFishById_InvalidId_ThrowsException()
        {
            // Arrange
            var koiFishId = 99;
            _unitOfWorkMock.Setup(uow => uow.KoiFishRepository.GetByIdAsync(koiFishId,
                It.IsAny<Expression<Func<KoiFish, object>>[]>())).ReturnsAsync((KoiFish)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetKoiFishById(koiFishId));
            Assert.Equal("404 - Fish not found!", exception.Message);
        }
        [Fact]
        public async Task CreateKoiFish_ReturnsKoiFishResponseDTO_WhenKoiFishIsCreatedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, RoleName = "CUSTOMER" };

            var fishModel = new KoiFishCreateDTO
            {
                Name = "Sakura",
                Origin = "Japan",
                Length = 60,
                Weight = 5000,
                Price = 1500000,
                KoiBreedIds = new List<int> { 1, 2 },
                ImageUrls = new List<string> { "http://example.com/image1.jpg", "http://example.com/image2.jpg" }
            };

            var koiFish = new KoiFish
            {
                Id = 1,
                Name = fishModel.Name,
                Origin = fishModel.Origin,
                Length = fishModel.Length,
                Weight = fishModel.Weight,
                Price = fishModel.Price,
                KoiBreeds = new List<KoiBreed>(),
                KoiFishImages = new List<KoiFishImage>()
            };

            var koiBreed1 = new KoiBreed { Id = 1, Name = "Showa" };
            var koiBreed2 = new KoiBreed { Id = 2, Name = "Kohaku" };

            _claimsServiceMock.Setup(cs => cs.GetCurrentUserId).Returns(userId);
            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetAccountDetailsAsync(userId))
                .ReturnsAsync(user);
            _unitOfWorkMock.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(1))
                .ReturnsAsync(koiBreed1);
            _unitOfWorkMock.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(2))
                .ReturnsAsync(koiBreed2);
            _unitOfWorkMock.Setup(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>()))
                .ReturnsAsync(koiFish);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync())
                .ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<KoiFish>(fishModel)).Returns(koiFish);
            _mapperMock.Setup(m => m.Map<KoiFishResponseDTO>(koiFish)).Returns(new KoiFishResponseDTO
            {
                Id = koiFish.Id,
                Name = koiFish.Name,
                Origin = koiFish.Origin,
                Length = koiFish.Length,
                Weight = koiFish.Weight,
                Price = koiFish.Price
            });

            // Act
            var result = await _service.CreateKoiFish(fishModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fishModel.Name, result.Name);
            Assert.Equal(fishModel.Origin, result.Origin);
            Assert.Equal(fishModel.Length, result.Length);
            Assert.Equal(fishModel.Weight, result.Weight);
            Assert.Equal(fishModel.Price, result.Price);

            // Verify calls
            _unitOfWorkMock.Verify(uow => uow.UserRepository.GetAccountDetailsAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiBreedRepository.GetByIdAsync(1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiBreedRepository.GetByIdAsync(2), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<KoiFish>(fishModel), Times.Once);
            _mapperMock.Verify(m => m.Map<KoiFishResponseDTO>(koiFish), Times.Once);
        }

        [Fact]
        public async Task CreateKoiFish_ThrowsException_WhenKoiBreedNotFound()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, RoleName = "CUSTOMER" };

            var fishModel = new KoiFishCreateDTO
            {
                Name = "Sakura",
                Origin = "Japan",
                Length = 60,
                Weight = 5000,
                KoiBreedIds = new List<int> { 999 },
                ImageUrls = new List<string> { "http://example.com/image1.jpg", "http://example.com/image2.jpg" }
            };
            var koiFish = new KoiFish
            {
                Id = 1,
                Name = fishModel.Name,
                Origin = fishModel.Origin,
                Length = fishModel.Length,
                Weight = fishModel.Weight,
                Price = fishModel.Price,
                KoiBreeds = new List<KoiBreed>(),
                KoiFishImages = new List<KoiFishImage>()
            };

            _claimsServiceMock.Setup(cs => cs.GetCurrentUserId).Returns(userId);
            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetAccountDetailsAsync(userId))
                .ReturnsAsync(user);
            _breedRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((KoiBreed)null);
            /*_unitOfWorkMock.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(999))
                .ReturnsAsync((KoiBreed)null);*/ // Simulate that the breed does not exist

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateKoiFish(fishModel));
            Assert.Equal("400 - Invalid Koi breed", exception.Message);

            _unitOfWorkMock.Verify(uow => uow.UserRepository.GetAccountDetailsAsync(userId), Times.Once);
           // _unitOfWorkMock.Verify(uow => uow.KoiBreedRepository.GetByIdAsync(999), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Never);
            _breedRepositoryMock.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateKoiFish_ThrowsException_WhenSaveChangesFails()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, RoleName = "CUSTOMER" };

            var fishModel = new KoiFishCreateDTO
            {
                Name = "Sakura",
                KoiBreedIds = new List<int> { 1, 2 },
            };

            var koiFish = new KoiFish
            {
                Id = 1,
                Name = fishModel.Name,
                KoiBreeds = new List<KoiBreed>(),
                KoiFishImages = new List<KoiFishImage>()
            };

            var koiBreed1 = new KoiBreed { Id = 1, Name = "Showa" };
            var koiBreed2 = new KoiBreed { Id = 2, Name = "Kohaku" };

            //_claimsServiceMock.Setup(cs => cs.GetCurrentUserId).Returns(userId);
            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetAccountDetailsAsync(userId))
                .ReturnsAsync(user);
            _unitOfWorkMock.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(1))
                .ReturnsAsync(koiBreed1);
            _unitOfWorkMock.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(2))
                .ReturnsAsync(koiBreed2);
            _unitOfWorkMock.Setup(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>()))
                .ReturnsAsync(koiFish);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync())
                .ReturnsAsync(0); // Simulate a save failure

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateKoiFish(fishModel));
            Assert.Equal("400 - Fail saving changes!", exception.Message);

            _unitOfWorkMock.Verify(uow => uow.UserRepository.GetAccountDetailsAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiBreedRepository.GetByIdAsync(1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiBreedRepository.GetByIdAsync(2), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task CreateKoiFish_CreatesKoiFishWithDefaultValues_WhenUserIsCustomer()
        {
            // Arrange
            var userId = 1;
            var user = new User { RoleName = "CUSTOMER", Id = userId };

            var fishModel = new KoiFishCreateDTO
            {
                Name = "Koi Fish",
                Origin = "Japan",
                Gender = "Male",
                Age = 2,
                Length = 60,
                Weight = 5000,
                IsAvailableForSale = false,
                Price = 1500000,
                IsSold = false,
                PersonalityTraits = "Friendly",
                DailyFeedAmount = 5,
                LastHealthCheck = DateTime.Now.AddDays(-7),
                KoiBreedIds = new List<int> { 1 },
                ImageUrls = new List<string> { "http://example.com/image.jpg" }
            };

            var koiBreed = new KoiBreed { Id = 1, Name = "Showa" };

            // Mocking claims and user repository
            _claimsServiceMock.Setup(cs => cs.GetCurrentUserId).Returns(userId);
            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetAccountDetailsAsync(userId)).ReturnsAsync(user);

            // Mocking KoiBreeds retrieval
            _unitOfWorkMock.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(1)).ReturnsAsync(koiBreed);

            // Create KoiFish instance with values from the DTO
            var koiFish = new KoiFish
            {
                Id = 1,
                Name = fishModel.Name,
                Origin = fishModel.Origin,
                Gender = fishModel.Gender == "Male", // Mapping gender
                Length = fishModel.Length,
                Weight = fishModel.Weight,
                IsAvailableForSale = fishModel.IsAvailableForSale ?? false,
                Price = fishModel.Price,
                IsSold = fishModel.IsSold ?? false,
                PersonalityTraits = fishModel.PersonalityTraits,
                DailyFeedAmount = fishModel.DailyFeedAmount,
                LastHealthCheck = fishModel.LastHealthCheck,
                KoiBreeds = new List<KoiBreed> { koiBreed },
                KoiFishImages = new List<KoiFishImage>() // Initialize this as necessary
            };

            // Mocking the mapping
            _mapperMock.Setup(m => m.Map<KoiFish>(fishModel)).Returns(koiFish);
            _unitOfWorkMock.Setup(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>())).ReturnsAsync(koiFish);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.CreateKoiFish(fishModel);

            // Assert
           Assert.NotNull(result); // Check if the result is not null
            Assert.Equal(koiFish.Id, result.Id);
            Assert.Equal(koiFish.Name, result.Name);
            Assert.Equal(koiFish.Origin, result.Origin);
            Assert.Equal(koiFish.Gender.HasValue && koiFish.Gender.Value ? "Male" : "Female", result.Gender);
            Assert.Equal(koiFish.Length, result.Length);
            Assert.Equal(koiFish.Weight, result.Weight);
            Assert.Equal(koiFish.Price, result.Price);
            Assert.Equal(koiFish.PersonalityTraits, result.PersonalityTraits);
            Assert.Equal(koiFish.DailyFeedAmount, result.DailyFeedAmount);
            Assert.Equal(koiFish.LastHealthCheck, result.LastHealthCheck);

            // Verify calls
            _unitOfWorkMock.Verify(uow => uow.UserRepository.GetAccountDetailsAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiBreedRepository.GetByIdAsync(1), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiFishRepository.AddAsync(It.IsAny<KoiFish>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }





    }
}
