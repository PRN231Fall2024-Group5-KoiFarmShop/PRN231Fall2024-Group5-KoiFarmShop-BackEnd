using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiBreedDTOs;
using Koi.Repositories.Interfaces;
using Koi.Services.Services;
using Koi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Koi.Repositories.Helper;

namespace Koi.Tests.Breed
{
    public class KoiBreedServiceTests: SetupTest
    {
        private readonly IKoiBreedService _koiBreedService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IClaimsService> _mockClaimsService;
        private readonly Mock<IKoiBreedRepository> _koiBreedRepository;

        public KoiBreedServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockClaimsService = new Mock<IClaimsService>();
            _koiBreedRepository = new Mock<IKoiBreedRepository>();
            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository).Returns(_koiBreedRepository.Object);

            _koiBreedService = new KoiBreedService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockClaimsService.Object
            );
        }

        [Fact]
        public void GetKoiBreeds_ShouldReturnAllKoiBreeds()
        {
            // Arrange
            var koiBreeds = new List<KoiBreed>
        {
            new KoiBreed { Id = 1, Name = "Koi Breed A", Content = "Content A", ImageUrl = "urlA" },
            new KoiBreed { Id = 2, Name = "Koi Breed B", Content = "Content B", ImageUrl = "urlB" }
        }.AsQueryable();

            var mockRepo = new Mock<IKoiBreedRepository>();
            mockRepo.Setup(repo => repo.GetQueryable()).Returns(koiBreeds);
            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository).Returns(mockRepo.Object);

            // Act
            var result = _koiBreedService.GetKoiBreeds().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Koi Breed A", result[0].Name);
            Assert.Equal("Koi Breed B", result[1].Name);
        }
        [Fact]
        public async Task GetKoiBreedById_ValidId_ReturnsKoiBreedResponseDTO()
        {
            // Arrange
            var koiBreedId = 1;
            var koiBreed = new KoiBreed
            {
                Id = koiBreedId,
                Name = "Koi Breed A",
                Content = "Content A",
                ImageUrl = "urlA"
            };

            var expectedResponse = new KoiBreedResponseDTO
            {
                Id = koiBreedId,
                Name = "Koi Breed A",
                Content = "Content A",
                ImageUrl = "urlA",
                IsDeleted = false
            };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(koiBreedId))
                .ReturnsAsync(koiBreed);

            _mockMapper.Setup(m => m.Map<KoiBreedResponseDTO>(koiBreed))
                .Returns(expectedResponse);

            // Act
            var result = await _koiBreedService.GetKoiBreedById(koiBreedId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            Assert.Equal(expectedResponse.Name, result.Name);
            Assert.Equal(expectedResponse.Content, result.Content);
            Assert.Equal(expectedResponse.ImageUrl, result.ImageUrl);
        }

        [Fact]
        public async Task GetKoiBreedById_InvalidId_ThrowsException()
        {
            // Arrange
            var invalidKoiBreedId = 999;

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(invalidKoiBreedId))
                .ReturnsAsync((KoiBreed)null); // Simulate no KoiBreed found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _koiBreedService.GetKoiBreedById(invalidKoiBreedId));
            Assert.Equal("404 - Breed not found!", exception.Message);
        }
        [Fact]
        public async Task CreateKoiBreed_ValidModel_ReturnsKoiBreedResponseDTO()
        {
            // Arrange
            var koiBreedCreateModel = new KoiBreedCreateDTO
            {
                Name = "Koi Breed A",
                Content = "Content A",
                ImageUrl = "urlA"
            };

            var koiBreed = new KoiBreed
            {
                Id = 1,
                Name = koiBreedCreateModel.Name,
                Content = koiBreedCreateModel.Content,
                ImageUrl = koiBreedCreateModel.ImageUrl,
            };

            var newKoiBreedResponse = new KoiBreedResponseDTO
            {
                Id = koiBreed.Id,
                Name = koiBreed.Name,
                Content = koiBreed.Content,
                ImageUrl = koiBreed.ImageUrl,
                IsDeleted = false
            };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetAllAsync(It.IsAny<Expression<Func<KoiBreed, bool>>>()))
        .ReturnsAsync(new List<KoiBreed>()); // No existing breeds

            // Set up the mock for AddAsync
            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.AddAsync(It.IsAny<KoiBreed>()))
                .ReturnsAsync(koiBreed); // Return the newly created KoiBreed

            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync())
                .ReturnsAsync(1); // Simulate successful save

            _mockMapper.Setup(m => m.Map<KoiBreedResponseDTO>(koiBreed))
                .Returns(newKoiBreedResponse);

            // Act
            var result = await _koiBreedService.CreateKoiBreed(koiBreedCreateModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newKoiBreedResponse.Id, result.Id);
            Assert.Equal(newKoiBreedResponse.Name, result.Name);
            Assert.Equal(newKoiBreedResponse.Content, result.Content);
            Assert.Equal(newKoiBreedResponse.ImageUrl, result.ImageUrl);
        }

        [Fact]
        public async Task CreateKoiBreed_BreedAlreadyExists_ThrowsException()
        {
            // Arrange
            var koiBreedCreateModel = new KoiBreedCreateDTO
            {
                Name = "Koi Breed A",
                Content = "Content A",
                ImageUrl = "urlA"
            };

            var existingKoiBreed = new KoiBreed
            {
                Id = 1, // Optionally set an Id for the existing breed
                Name = koiBreedCreateModel.Name,
                Content = koiBreedCreateModel.Content,
                ImageUrl = koiBreedCreateModel.ImageUrl,
            };

            // Set up the mock to return the existing breed using a predicate
            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetAllAsync(It.IsAny<Expression<Func<KoiBreed, bool>>>()))
                .ReturnsAsync(new List<KoiBreed> { existingKoiBreed }); // Simulate existing breed

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _koiBreedService.CreateKoiBreed(koiBreedCreateModel));
            Assert.Equal("400 - Create failed. Breed has already existed!", exception.Message);
        }

        [Fact]
        public async Task CreateKoiBreed_AdditionFails_ThrowsException()
        {
            // Arrange
            var koiBreedCreateModel = new KoiBreedCreateDTO
            {
                Name = "Koi Breed A",
                Content = "Content A",
                ImageUrl = "urlA"
            };

            // Set up the mock to return an empty list (no existing breeds) using a predicate
            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetAllAsync(It.IsAny<Expression<Func<KoiBreed, bool>>>()))
                .ReturnsAsync(new List<KoiBreed>()); // No existing breeds

            // Simulate successful addition
            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.AddAsync(It.IsAny<KoiBreed>()))
                .ReturnsAsync(new KoiBreed()); // Simulate addition

            // Simulate failed save
            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync())
                .ReturnsAsync(0); // Simulate failed save

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _koiBreedService.CreateKoiBreed(koiBreedCreateModel));
            Assert.Equal("500 - Adding process failed", exception.Message);
        }
        [Fact]
        public async Task UpdateKoiBreed_BreedExists_UpdatesSuccessfully()
        {
            // Arrange
            var id = 1;
            var koiBreedModel = new KoiBreedCreateDTO
            {
                Name = "Updated Koi Breed",
                Content = "Updated content",
                ImageUrl = "http://example.com/image.jpg"
            };

            var koiBreed = new KoiBreed
            {
                Id = id,
                Name = "Old Koi Breed",
                Content = "Old content"
            };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(id))
                .ReturnsAsync(koiBreed); // Mocking the retrieval of the Koi breed

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.Update(It.IsAny<KoiBreed>()))
                .ReturnsAsync(true); // Simulating successful update

            _mockMapper.Setup(m => m.Map<KoiBreedResponseDTO>(koiBreed))
                .Returns(new KoiBreedResponseDTO { Name = koiBreedModel.Name }); // Mocking the mapping

            _mockClaimsService.Setup(cs => cs.GetCurrentUserId).Returns(1); // Mocking current user ID

            // Act
            var result = await _koiBreedService.UpdateKoiBreed(id, koiBreedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(koiBreedModel.Name, result.Name);
            _mockUnitOfWork.Verify(uow => uow.KoiBreedRepository.Update(It.Is<KoiBreed>(kb => kb.Id == id && kb.Name == koiBreedModel.Name && kb.Content == koiBreedModel.Content && kb.ImageUrl == koiBreedModel.ImageUrl)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateKoiBreed_BreedNotFound_ThrowsException()
        {
            // Arrange
            var id = 1;
            var koiBreedModel = new KoiBreedCreateDTO
            {
                Name = "Updated Koi Breed",
                Content = "Updated content",
                ImageUrl = "http://example.com/image.jpg"
            };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(id))
                .ReturnsAsync((KoiBreed)null); // Mocking retrieval that returns null

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _koiBreedService.UpdateKoiBreed(id, koiBreedModel));
            Assert.Equal("404 - Update failed. Breed not found!", exception.Message);
        }

        [Fact]
        public async Task UpdateKoiBreed_UpdateFails_ThrowsException()
        {
            // Arrange
            var id = 1;
            var koiBreedModel = new KoiBreedCreateDTO
            {
                Name = "Updated Koi Breed",
                Content = "Updated content",
                ImageUrl = "http://example.com/image.jpg"
            };

            var koiBreed = new KoiBreed
            {
                Id = id,
                Name = "Old Koi Breed",
                Content = "Old content"
            };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(id))
                .ReturnsAsync(koiBreed); // Mocking the retrieval of the Koi breed

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.Update(It.IsAny<KoiBreed>()))
                .ReturnsAsync(false); // Simulating update failure

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _koiBreedService.UpdateKoiBreed(id, koiBreedModel));
            Assert.Equal("400 - Update failed", exception.Message);
        }
        [Fact]
        public async Task DeleteKoiBreed_BreedExists_DeletesSuccessfully()
        {
            // Arrange
            var id = 1;
            var koiBreed = new KoiBreed { Id = id, Name = "Koi Breed A" };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(id))
                .ReturnsAsync(koiBreed); // Mocking retrieval of the Koi breed

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.SoftRemove(koiBreed))
                .ReturnsAsync(true); // Simulating successful soft remove

            _mockClaimsService.Setup(cs => cs.GetCurrentUserId).Returns(1); // Mocking current user ID

            // Act
            var result = await _koiBreedService.DeleteKoiBreed(id);

            // Assert
            Assert.True(result); // Should return true indicating deletion
            _mockUnitOfWork.Verify(uow => uow.KoiBreedRepository.SoftRemove(koiBreed), Times.Once); // Verify SoftRemove was called once
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once); // Verify SaveChangeAsync was called once
        }

        [Fact]
        public async Task DeleteKoiBreed_BreedNotFound_ThrowsException()
        {
            // Arrange
            var id = 1;

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(id))
                .ReturnsAsync((KoiBreed)null); // Mocking retrieval that returns null

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _koiBreedService.DeleteKoiBreed(id));
            Assert.Equal("404 - Delete failed. Breed not found!", exception.Message);
        }

        [Fact]
        public async Task DeleteKoiBreed_SoftRemoveFails_ReturnsFalse()
        {
            // Arrange
            var id = 1;
            var koiBreed = new KoiBreed { Id = id, Name = "Koi Breed A" };

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.GetByIdAsync(id))
                .ReturnsAsync(koiBreed); // Mocking retrieval of the Koi breed

            _mockUnitOfWork.Setup(uow => uow.KoiBreedRepository.SoftRemove(koiBreed))
                .ReturnsAsync(false); // Simulating soft remove failure

            // Act
            var result = await _koiBreedService.DeleteKoiBreed(id);

            // Assert
            Assert.False(result); // Should return false indicating deletion failed
            _mockUnitOfWork.Verify(uow => uow.KoiBreedRepository.SoftRemove(koiBreed), Times.Once); // Verify SoftRemove was called once
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once); // Verify SaveChangeAsync was called once
        }


    }
}
