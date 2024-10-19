using Koi.BusinessObjects;
using Koi.DTOs.KoiDiaryDTOs;
using Koi.Services.Services;
using Koi.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Koi.Tests.Diary
{
    public class KoiDiaryServiceTests : SetupTest
    {
        private readonly IKoiDiaryService _service;
        private readonly Mock<IMapper> _mockMapper;

        public KoiDiaryServiceTests() : base()
        {
            // Initialize the KoiDiaryService with the mocked dependencies
            _mockMapper = new Mock<IMapper>();
            _service = new KoiDiaryService(
                _unitOfWorkMock.Object,
                _mockMapper.Object,
                _claimsServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateDiary_FishExistsAndNoDiaryForDate_ReturnsKoiFishDiaryCreateDTO()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryCreateDTO
            {
                KoiFishId = 1,
                Description = "Test description",
                Date = new DateTime(2024, 10, 19)
            };

            var koiFish = new KoiFish { Id = 1 }; // Mock fish entity
            var koiDiary = new KoiDiary(); // Mock diary entity

            _unitOfWorkMock.Setup(repo => repo.KoiFishRepository.GetByIdAsync(koiDiaryDTO.KoiFishId)).ReturnsAsync(koiFish);
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetAllAsync(null)).ReturnsAsync(new List<KoiDiary>());
            _mockMapper.Setup(mapper => mapper.Map<KoiDiary>(koiDiaryDTO)).Returns(koiDiary);
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.AddAsync(koiDiary)).ReturnsAsync(koiDiary);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1); // Simulate successful save

            // Act
            var result = await _service.CreateDiary(koiDiaryDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(koiDiaryDTO.KoiFishId, result.KoiFishId);
            _unitOfWorkMock.Verify(uow => uow.KoiFishRepository.GetByIdAsync(koiDiaryDTO.KoiFishId), Times.Once);
            _unitOfWorkMock.Verify(repo => repo.KoiDiaryRepository.AddAsync(It.IsAny<KoiDiary>()), Times.Once);
        }

        [Fact]
        public async Task CreateDiary_FishNotFound_ThrowsException()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryCreateDTO
            {
                KoiFishId = 1,
                Description = "Test description",
                Date = new DateTime(2024, 10, 19)
            };

            _unitOfWorkMock.Setup(repo => repo.KoiFishRepository.GetByIdAsync(koiDiaryDTO.KoiFishId)).ReturnsAsync((KoiFish)null); // No fish found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateDiary(koiDiaryDTO));
            Assert.Equal("400 - Fish not Found!", exception.Message);
        }

        [Fact]
        public async Task CreateDiary_DiaryAlreadyExists_ThrowsException()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryCreateDTO
            {
                KoiFishId = 1,
                Description = "Test description",
                Date = new DateTime(2024, 10, 19)
            };

            var koiFish = new KoiFish { Id = 1 }; // Mock fish entity
            var existingDiary = new KoiDiary
            {
                KoiFishId = 1,
                Date = new DateTime(2024, 10, 19)
            };

            _unitOfWorkMock.Setup(repo => repo.KoiFishRepository.GetByIdAsync(koiDiaryDTO.KoiFishId)).ReturnsAsync(koiFish);
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetAllAsync(null)).ReturnsAsync(new List<KoiDiary> { existingDiary }); // Existing diary

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateDiary(koiDiaryDTO));
            Assert.Equal("400 - Fish has already have diary on this date", exception.Message);
        }

        [Fact]
        public async Task CreateDiary_SaveChangesFails_ThrowsException()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryCreateDTO
            {
                KoiFishId = 1,
                Description = "Test description",
                Date = new DateTime(2024, 10, 19)
            };

            var koiFish = new KoiFish { Id = 1 };
            var koiDiary = new KoiDiary();

            _unitOfWorkMock.Setup(repo => repo.KoiFishRepository.GetByIdAsync(koiDiaryDTO.KoiFishId)).ReturnsAsync(koiFish);
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetAllAsync(null)).ReturnsAsync(new List<KoiDiary>());
            _mockMapper.Setup(mapper => mapper.Map<KoiDiary>(koiDiaryDTO)).Returns(koiDiary);
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.AddAsync(koiDiary)).ReturnsAsync(koiDiary);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(0); // Simulate failed save

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateDiary(koiDiaryDTO));
            Assert.Equal("400 - Fail saving changes", exception.Message);
        }
        [Fact]
        public void GetDiaryList_ReturnsExpectedDiaryList()
        {
            // Arrange
            var diaryList = new List<KoiDiary>
            {
                new KoiDiary { KoiFishId = 1, Description = "Test description 1", Date = new DateTime(2024, 10, 19) },
                new KoiDiary { KoiFishId = 2, Description = "Test description 2", Date = new DateTime(2024, 10, 20) }
            }.AsQueryable(); // Convert list to IQueryable

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetQueryable()).Returns(diaryList); // Mock repository


            // Act
            var result = _service.GetDiaryList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Check that two items are returned
            Assert.Equal("Test description 1", result.First().Description); // Check the first item's description
        }
        [Fact]
        public async Task UpdateDiary_DiaryExistsAndIsUpdatable_ReturnsUpdatedDiary()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryUpdateDTO
            {
                Description = "Updated description"
            };

            var koiDiary = new KoiDiary
            {
                Id = 1,
                Description = "Original description",
                Date = DateTime.Now.AddDays(-2) // within the 3-day update limit
            };

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiary.Id)).ReturnsAsync(koiDiary);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1); // Simulate successful save
            _mockMapper.Setup(mapper => mapper.Map<KoiFishDiaryCreateDTO>(It.IsAny<KoiDiary>()))
                       .Returns(new KoiFishDiaryCreateDTO { KoiFishId = koiDiary.Id, Description = koiDiaryDTO.Description, Date = koiDiary.Date });

            // Act
            var result = await _service.UpdateDiary(koiDiary.Id, koiDiaryDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(koiDiaryDTO.Description, result.Description); // Check that the description was updated
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiary.Id), Times.Exactly(2)); // Should be called twice
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateDiary_UpdateTimeExceeded_ThrowsException()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryUpdateDTO
            {
                Description = "Updated description"
            };

            var koiDiary = new KoiDiary
            {
                Id = 1,
                Description = "Original description",
                Date = DateTime.Now.AddDays(-4) // past the 3-day update limit
            };

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiary.Id)).ReturnsAsync(koiDiary);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateDiary(koiDiary.Id, koiDiaryDTO));
            Assert.Equal("400 - Update time is over!", exception.Message);
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiary.Id), Times.Once); // Should be called once before exception is thrown
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Never); // Save changes should not be called
        }

        [Fact]
        public async Task UpdateDiary_SaveChangesFails_ThrowsException()
        {
            // Arrange
            var koiDiaryDTO = new KoiFishDiaryUpdateDTO
            {
                Description = "Updated description"
            };

            var koiDiary = new KoiDiary
            {
                Id = 1,
                Description = "Original description",
                Date = DateTime.Now.AddDays(-2) // within the 3-day update limit
            };

            // Set up the initial call to fetch the diary for updating
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiary.Id))
                .ReturnsAsync(koiDiary)
                .Verifiable();

            // Simulate failed save
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(0);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateDiary(koiDiary.Id, koiDiaryDTO));

            // Ensure the exception message is as expected
            Assert.Equal("400 - Fail saving changes", exception.Message);

            // Verify that GetByIdAsync was only called once, since save failed
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiary.Id), Times.Once);

            // Verify SaveChangeAsync was called once
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteDiary_ValidId_ReturnsDeletedKoiFishDiaryCreateDTO()
        {
            // Arrange
            var koiDiaryId = 1;
            var koiDiary = new KoiDiary
            {
                Id = koiDiaryId,
                Date = DateTime.Now.AddDays(-2), // within 3-day limit
                KoiFishId = 1,
                Description = "Diary description",
                IsDeleted = false
            };

            var koiDiaryDTO = new KoiFishDiaryCreateDTO
            {
                KoiFishId = 1,
                Description = "Diary description",
                Date = koiDiary.Date
            };

            // Mock GetByIdAsync to return the koiDiary
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiaryId))
                .ReturnsAsync(koiDiary)
                .Verifiable();

            // Mock the SoftRemove to return true, indicating successful soft delete
            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.SoftRemove(koiDiary))
                .ReturnsAsync(true)
                .Verifiable();

            // Mock SaveChangeAsync to return > 0, indicating successful save
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync())
                .ReturnsAsync(1)
                .Verifiable();

            // Mock Mapper to map the deleted diary back to DTO
            _mockMapper.Setup(mapper => mapper.Map<KoiFishDiaryCreateDTO>(koiDiary))
                .Returns(koiDiaryDTO)
                .Verifiable();

            // Act
            var result = await _service.DeleteDiary(koiDiaryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(koiDiaryDTO.KoiFishId, result.KoiFishId);
            Assert.Equal(koiDiaryDTO.Description, result.Description);

            // Verify interactions
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiaryId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.SoftRemove(koiDiary), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<KoiFishDiaryCreateDTO>(koiDiary), Times.Once);
        }

        [Fact]
        public async Task DeleteDiary_OutsideUpdateTime_ThrowsException()
        {
            // Arrange
            var koiDiaryId = 1;
            var koiDiary = new KoiDiary
            {
                Id = koiDiaryId,
                Date = DateTime.Now.AddDays(-5), // outside the 3-day limit
                KoiFishId = 1,
                Description = "Diary description",
                IsDeleted = false
            };

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiaryId))
                .ReturnsAsync(koiDiary)
                .Verifiable();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteDiary(koiDiaryId));

            Assert.Equal("400 - Update time is over!", exception.Message);

            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiaryId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Never); // No save because exception occurs
        }

        [Fact]
        public async Task DeleteDiary_SaveChangesFails_ThrowsException()
        {
            // Arrange
            var koiDiaryId = 1;
            var koiDiary = new KoiDiary
            {
                Id = koiDiaryId,
                Date = DateTime.Now.AddDays(-2), // within 3-day limit
                KoiFishId = 1,
                Description = "Diary description",
                IsDeleted = false
            };

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiaryId))
                .ReturnsAsync(koiDiary)
                .Verifiable();

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.SoftRemove(koiDiary))
                .ReturnsAsync(true)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync())
                .ReturnsAsync(0); // Simulate failed save

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteDiary(koiDiaryId));

            Assert.Equal("400 - Fail saving changes", exception.Message);

            // Verify interactions
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiaryId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.SoftRemove(koiDiary), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteDiary_SoftRemoveFails_ThrowsException()
        {
            // Arrange
            var koiDiaryId = 1;
            var koiDiary = new KoiDiary
            {
                Id = koiDiaryId,
                Date = DateTime.Now.AddDays(-2), // within 3-day limit
                KoiFishId = 1,
                Description = "Diary description",
                IsDeleted = false
            };

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.GetByIdAsync(koiDiaryId))
                .ReturnsAsync(koiDiary)
                .Verifiable();

            _unitOfWorkMock.Setup(repo => repo.KoiDiaryRepository.SoftRemove(koiDiary))
                .ReturnsAsync(false); // Simulate failure in soft delete

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteDiary(koiDiaryId));

            Assert.Equal("400 - Fail saving changes", exception.Message);

            // Verify interactions
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.GetByIdAsync(koiDiaryId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.KoiDiaryRepository.SoftRemove(koiDiary), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Never); // No save because soft delete failed
        }


    }
}
