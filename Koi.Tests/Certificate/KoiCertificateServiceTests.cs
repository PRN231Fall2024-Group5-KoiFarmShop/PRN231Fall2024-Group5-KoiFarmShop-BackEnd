using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Koi.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Koi.Tests.Certificate
{
    public class KoiCertificateServiceTests : SetupTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IKoiCertificateRepository> _mockKoiCertificateRepository;
        private readonly IKoiCertificateService _mockKoiCertificateService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IKoiFishRepository> _mockFishRepository;

        public KoiCertificateServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockKoiCertificateRepository = new Mock<IKoiCertificateRepository>();
            _mockFishRepository = new Mock<IKoiFishRepository>();
            _mockMapper = new Mock<IMapper>();

            // Set up the UnitOfWork to return the mock repository
            _mockUnitOfWork.Setup(uow => uow.KoiCertificateRepository).Returns(_mockKoiCertificateRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.KoiFishRepository).Returns(_mockFishRepository.Object);

            // Initialize the service with the mocked dependencies
            _mockKoiCertificateService = new KoiCertificateService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetKoiCertificateById_CertificateExists_ReturnsMappedDTO()
        {
            // Arrange
            int certificateId = 1;
            var koiCertificate = new KoiCertificate
            {
                Id = certificateId,
                KoiFishId = 1,
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };
            var koiCertificateDTO = new KoiCertificateResponseDTO
            {
                Id = certificateId,
                KoiFishId = 1,
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };

            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(certificateId))
                .ReturnsAsync(koiCertificate);

            _mockMapper.Setup(m => m.Map<KoiCertificateResponseDTO>(koiCertificate))
                .Returns(koiCertificateDTO);

            // Act
            var result = await _mockKoiCertificateService.GetKoiCertificateById(certificateId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(koiCertificateDTO.Id, result.Id);
            Assert.Equal(koiCertificateDTO.CertificateType, result.CertificateType);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(certificateId), Times.Once);
        }

        [Fact]
        public async Task GetKoiCertificateById_CertificateNotFound_ThrowsException()
        {
            // Arrange
            int certificateId = 999; // Non-existent ID

            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(certificateId))
                .ReturnsAsync((KoiCertificate)null); // No certificate found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.GetKoiCertificateById(certificateId));

            Assert.Equal("404 - Certificate not found!", exception.Message);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(certificateId), Times.Once);
        }

        [Fact]
        public async Task GetListCertificateByKoiId_CertificateListExists_ReturnsMappedDTOList()
        {
            // Arrange
            int koiId = 1;
            var koiCertificates = new List<KoiCertificate>
            {
                new KoiCertificate
                {
                    Id = 1,
                    KoiFishId = koiId,
                    CertificateType = "TypeA",
                    CertificateUrl = "http://example.com/certificateA"
                },
                new KoiCertificate
                {
                    Id = 2,
                    KoiFishId = koiId,
                    CertificateType = "TypeB",
                    CertificateUrl = "http://example.com/certificateB"
                }
            };

            var koiCertificateDTOs = new List<KoiCertificateResponseDTO>
            {
                new KoiCertificateResponseDTO
                {
                    Id = 1,
                    KoiFishId = koiId,
                    CertificateType = "TypeA",
                    CertificateUrl = "http://example.com/certificateA"
                },
                new KoiCertificateResponseDTO
                {
                    Id = 2,
                    KoiFishId = koiId,
                    CertificateType = "TypeB",
                    CertificateUrl = "http://example.com/certificateB"
                }
            };

            _mockKoiCertificateRepository.Setup(repo => repo.GetListCertificateByKoiIdAsync(koiId))
                .ReturnsAsync(koiCertificates);

            _mockMapper.Setup(m => m.Map<List<KoiCertificateResponseDTO>>(koiCertificates))
                .Returns(koiCertificateDTOs);

            // Act
            var result = await _mockKoiCertificateService.GetListCertificateByKoiId(koiId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(koiCertificateDTOs[0].CertificateType, result[0].CertificateType);
            Assert.Equal(koiCertificateDTOs[1].CertificateUrl, result[1].CertificateUrl);
            _mockKoiCertificateRepository.Verify(repo => repo.GetListCertificateByKoiIdAsync(koiId), Times.Once);
        }

        [Fact]
        public async Task GetListCertificateByKoiId_CertificateListNotFound_ThrowsException()
        {
            // Arrange
            int koiId = 999; // Non-existent Koi ID

            _mockKoiCertificateRepository.Setup(repo => repo.GetListCertificateByKoiIdAsync(koiId))
                .ReturnsAsync((List<KoiCertificate>)null); // No certificates found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.GetListCertificateByKoiId(koiId));

            Assert.Equal("404 - Certificate not found!", exception.Message);
            _mockKoiCertificateRepository.Verify(repo => repo.GetListCertificateByKoiIdAsync(koiId), Times.Once);
        }

        [Fact]
        public async Task CreateKoiCertificate_ShouldReturnCreatedCertificate()
        {
            // Arrange
            var dto = new CreateKoiCertificateDTO
            {
                KoiFishId = 1,
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };

            var koiFish = new KoiFish { Id = dto.KoiFishId }; // Mock the KoiFish entity
            _mockFishRepository.Setup(repo => repo.GetByIdAsync(dto.KoiFishId))
                .ReturnsAsync(koiFish);

            var koiCertificate = new KoiCertificate
            {
                KoiFishId = dto.KoiFishId,
                CertificateType = dto.CertificateType,
                CertificateUrl = dto.CertificateUrl
            };

            _mockKoiCertificateRepository.Setup(repo => repo.AddAsync(It.IsAny<KoiCertificate>()))
                .ReturnsAsync(koiCertificate);

            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1); // Simulate successful save

            // Mock the mapper to return a valid response
            var expectedResponse = new KoiCertificateResponseDTO
            {
                KoiFishId = koiCertificate.KoiFishId,
                CertificateType = koiCertificate.CertificateType,
                CertificateUrl = koiCertificate.CertificateUrl
            };

            // Set up the mapper to return the expected response when mapping
            _mockMapper.Setup(m => m.Map<KoiCertificateResponseDTO>(koiCertificate)).Returns(expectedResponse);

            // Act
            var result = await _mockKoiCertificateService.CreateKoiCertificate(dto);

            // Assert
            Assert.NotNull(result); // This should not fail now
            Assert.Equal(dto.CertificateType, result.CertificateType);
            Assert.Equal(dto.CertificateUrl, result.CertificateUrl);
            _mockFishRepository.Verify(repo => repo.GetByIdAsync(dto.KoiFishId), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.AddAsync(It.IsAny<KoiCertificate>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateKoiCertificate_KoiFishDoesNotExist_ThrowsException()
        {
            // Arrange
            var dto = new CreateKoiCertificateDTO
            {
                KoiFishId = 999, // Non-existent KoiFish ID
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };

            _mockFishRepository.Setup(repo => repo.GetByIdAsync(dto.KoiFishId))
                .ReturnsAsync((KoiFish)null); // No KoiFish found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.CreateKoiCertificate(dto));

            Assert.Equal("400 - Create failed. KoiFish does not exist!", exception.Message);
            _mockFishRepository.Verify(repo => repo.GetByIdAsync(dto.KoiFishId), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.AddAsync(It.IsAny<KoiCertificate>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateKoiCertificate_SaveFails_ThrowsException()
        {
            // Arrange
            var dto = new CreateKoiCertificateDTO
            {
                KoiFishId = 1,
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };

            var koiFish = new KoiFish { Id = dto.KoiFishId };
            _mockFishRepository.Setup(repo => repo.GetByIdAsync(dto.KoiFishId)).ReturnsAsync(koiFish);

            var koiCertificate = new KoiCertificate
            {
                KoiFishId = dto.KoiFishId,
                CertificateType = dto.CertificateType,
                CertificateUrl = dto.CertificateUrl
            };

            _mockKoiCertificateRepository.Setup(repo => repo.AddAsync(It.IsAny<KoiCertificate>())).ReturnsAsync(koiCertificate);
            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(0); // Simulate failed save

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.CreateKoiCertificate(dto));

            Assert.Equal("400 - Create failed", exception.Message); // Check that the correct message is thrown
            _mockFishRepository.Verify(repo => repo.GetByIdAsync(dto.KoiFishId), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.AddAsync(It.IsAny<KoiCertificate>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateKoiCertificate_ShouldReturnUpdatedCertificate()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateKoiCertificateDTO
            {
                CertificateType = "TypeB",
                CertificateUrl = "http://example.com/updated-certificate"
            };

            var existingCertificate = new KoiCertificate
            {
                Id = id,
                KoiFishId = 1,
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };

            // Mock repository to return the existing certificate
            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(existingCertificate);

            // Mock repository to simulate successful update
            _mockKoiCertificateRepository.Setup(repo => repo.Update(It.IsAny<KoiCertificate>()))
                .ReturnsAsync(true); // Ensure the mock returns true

            // Mock SaveChangeAsync to simulate successful save
            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1);

            // Mock the mapping between KoiCertificate and KoiCertificateResponseDTO
            var expectedResponse = new KoiCertificateResponseDTO
            {
                Id = id,
                KoiFishId = existingCertificate.KoiFishId,
                CertificateType = dto.CertificateType,
                CertificateUrl = dto.CertificateUrl
            };

            _mockMapper.Setup(m => m.Map<KoiCertificateResponseDTO>(It.IsAny<KoiCertificate>()))
                .Returns(expectedResponse); // Ensure the mapper returns a valid response

            // Act
            var result = await _mockKoiCertificateService.UpdateKoiCertificate(dto, id);

            // Assert
            Assert.NotNull(result); // Ensure result is not null
            Assert.Equal(dto.CertificateType, result.CertificateType);
            Assert.Equal(dto.CertificateUrl, result.CertificateUrl);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.Update(It.IsAny<KoiCertificate>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateKoiCertificate_CertificateDoesNotExist_ThrowsException()
        {
            // Arrange
            var id = 999; // Non-existent certificate ID
            var dto = new UpdateKoiCertificateDTO
            {
                CertificateType = "TypeB",
                CertificateUrl = "http://example.com/updated-certificate"
            };

            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((KoiCertificate)null); // Simulate no certificate found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.UpdateKoiCertificate(dto, id));

            Assert.Equal("400 - Update failed", exception.Message);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.Update(It.IsAny<KoiCertificate>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateKoiCertificate_SaveFails_ThrowsException()
        {
            // Arrange
            var id = 1;
            var dto = new UpdateKoiCertificateDTO
            {
                CertificateType = "TypeB",
                CertificateUrl = "http://example.com/updated-certificate"
            };

            var existingCertificate = new KoiCertificate
            {
                Id = id,
                KoiFishId = 1,
                CertificateType = "TypeA",
                CertificateUrl = "http://example.com/certificate"
            };

            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(existingCertificate);

            _mockKoiCertificateRepository.Setup(repo => repo.Update(It.IsAny<KoiCertificate>()))
                .ReturnsAsync(true); // Simulate successful update

            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(0); // Simulate failed save

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.UpdateKoiCertificate(dto, id));

            Assert.Equal("400 - Update failed", exception.Message);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.Update(It.IsAny<KoiCertificate>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteKoiCertificate_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            // Arrange
            var id = 1;
            var existingCertificate = new KoiCertificate
            {
                Id = id,
                IsDeleted = false
            };

            // Mock repository to return the existing certificate
            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(existingCertificate);

            // Mock SoftRemove to simulate successful deletion
            _mockKoiCertificateRepository.Setup(repo => repo.SoftRemove(It.IsAny<KoiCertificate>()))
                .ReturnsAsync(true);

            // Mock SaveChangeAsync to simulate successful save
            _mockUnitOfWork.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1);

            // Act
            var result = await _mockKoiCertificateService.DeleteKoiCertificate(id);

            // Assert
            Assert.True(result); // Deletion was successful
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.SoftRemove(existingCertificate), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteKoiCertificate_ShouldThrowException_WhenCertificateDoesNotExist()
        {
            // Arrange
            var id = 999; // Non-existent KoiCertificate ID

            // Mock repository to return null (certificate not found)
            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((KoiCertificate)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.DeleteKoiCertificate(id));

            Assert.Equal("400 - Delete failed", exception.Message);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.SoftRemove(It.IsAny<KoiCertificate>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteKoiCertificate_ShouldThrowException_WhenSoftRemoveFails()
        {
            // Arrange
            var id = 1;
            var existingCertificate = new KoiCertificate
            {
                Id = id,
                IsDeleted = false
            };

            // Mock repository to return the existing certificate
            _mockKoiCertificateRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(existingCertificate);

            // Mock SoftRemove to simulate failure
            _mockKoiCertificateRepository.Setup(repo => repo.SoftRemove(It.IsAny<KoiCertificate>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockKoiCertificateService.DeleteKoiCertificate(id));

            Assert.Equal("400 - Delete failed", exception.Message);
            _mockKoiCertificateRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _mockKoiCertificateRepository.Verify(repo => repo.SoftRemove(existingCertificate), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Never);
        }
        [Fact]
        public void GetKoiCertificates_ShouldReturnQueryableKoiCertificates()
        {
            // Arrange
            var certificates = new List<KoiCertificate>
            {
                new KoiCertificate { Id = 1, CertificateUrl = "http://example.com/certificate1", CertificateType = "TypeA" },
                new KoiCertificate { Id = 2, CertificateUrl = "http://example.com/certificate2", CertificateType = "TypeB" }
            }.AsQueryable(); // Convert the list to IQueryable

            // Mock the repository to return the certificates as IQueryable
            _mockKoiCertificateRepository.Setup(repo => repo.GetQueryable())
                .Returns(certificates);

            // Act
            var result = _mockKoiCertificateService.GetKoiCertificates();

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.IsAssignableFrom<IQueryable<KoiCertificate>>(result); // Ensure the result is IQueryable

            // Optionally, you can also verify the contents of the IQueryable
            var resultList = result.ToList(); // Convert IQueryable to List to examine contents
            Assert.Equal(2, resultList.Count); // Verify that we have 2 items
            Assert.Equal("http://example.com/certificate1", resultList[0].CertificateUrl);
            Assert.Equal("http://example.com/certificate2", resultList[1].CertificateUrl);

            _mockKoiCertificateRepository.Verify(repo => repo.GetQueryable(), Times.Once);
        }
        



    }
}
