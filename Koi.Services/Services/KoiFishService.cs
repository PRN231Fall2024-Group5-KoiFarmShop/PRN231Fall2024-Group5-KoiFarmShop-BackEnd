using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiFishDTOs;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class KoiFishService : IKoiFishService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IClaimsService _claimsService;
        private readonly IKoiCertificateService _koiCertificateService;

        public KoiFishService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimsService claimsService,
            IKoiCertificateService koiCertificateService
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _koiCertificateService = koiCertificateService;
        }

        public async Task<PagedList<KoiFish>> GetKoiFishes(KoiParams koiParams)
        {
            var query = _unitOfWork.KoiFishRepository.FilterAllField(koiParams).AsQueryable();
            var fishes = await PagedList<KoiFish>.ToPagedList(query, koiParams.PageNumber, koiParams.PageSize);

            return fishes;
        }

        public IQueryable<KoiFish> GetKoiFishes()
        {
            return _unitOfWork.KoiFishRepository.FilterAllField();
        }

        public IQueryable<KoiFish> GetMyKoiFishes()
        {
            var id = _claimsService.GetCurrentUserId;
            return _unitOfWork.KoiFishRepository.FilterAllField().Where(x => x.OwnerId == id).AsQueryable();
        }

        public async Task<KoiFishResponseDTO> GetKoiFishById(int id)
        {
            var fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id,
                x => x.KoiBreeds.Where(x => x.IsDeleted == false),
                x => x.KoiFishImages.Where(x => x.IsDeleted == false),
                x => x.KoiDiaries.Where(x => x.IsDeleted == false),
                x => x.KoiCertificates.Where(x => x.IsDeleted == false),
                x => x.Owner);

            if (fish == null)
            {
                throw new Exception("404 - Fish not found!");
            }

            var result = _mapper.Map<KoiFishResponseDTO>(fish);

            return result;
        }

        public async Task<KoiFishResponseDTO> CreateKoiFish(KoiFishCreateDTO fishModel)
        {
            var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (user == null)
            {
                throw new Exception("401 - User is not signed in ");
            }
            KoiFish fish = _mapper.Map<KoiFish>(fishModel);
            fish.KoiBreeds = [];
            foreach (var breedId in fishModel.KoiBreedIds)
            {
                var breed = await _unitOfWork.KoiBreedRepository.GetByIdAsync(breedId);
                if (breed == null)
                {
                    throw new Exception("400 - Invalid Koi breed");
                }
                fish.KoiBreeds.Add(_mapper.Map<KoiBreed>(breed));
            }
            fish.KoiFishImages = [];
            foreach (var item in fishModel.ImageUrls)
            {
                fish.KoiFishImages.Add(new KoiFishImage
                {
                    ImageUrl = item
                });
            }
            if (user.RoleName == "CUSTOMER")
            {
                fish.IsAvailableForSale = false;
                fish.IsDeleted = false;
                fish.Price = fishModel.Price;
                fish.OwnerId = _claimsService.GetCurrentUserId;
            }
            fish.IsAvailableForSale = fishModel.IsAvailableForSale;
            fish.IsDeleted = false;

            var result = await _unitOfWork.KoiFishRepository.AddAsync(fish);

            if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes!");

            if (fishModel.Certificates != null && fishModel.Certificates.Any())
            {
                foreach (var certInfo in fishModel.Certificates)
                {
                    var certificateDto = new CreateKoiCertificateDTO
                    {
                        CertificateUrl = certInfo.CertificateUrl,
                        CertificateType = certInfo.CertificateType,
                        KoiFishId = result.Id
                    };

                    await _koiCertificateService.CreateKoiCertificate(certificateDto);
                }
            }

            return _mapper.Map<KoiFishResponseDTO>(result);
        }

        public async Task<KoiFishResponseDTO> UpdateKoiFish(int id, KoiFishUpdateDTO fishModel)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
                KoiFish fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id, x => x.KoiBreeds);
                if (user.RoleName == "CUSTOMER" && fish.OwnerId != null && user.Id != fish.OwnerId) throw new Exception("403 - Forbiden");
                foreach (var breedId in fishModel.KoiBreedIds)
                {
                    var breed = await _unitOfWork.KoiBreedRepository.GetByIdAsync(breedId);
                    if (breed == null)
                    {
                        throw new Exception("400 - Invalid Koi breed");
                    }
                    fish.KoiBreeds.Add(breed);
                }
                fish.Length = fishModel.Length;
                fish.Weight = fishModel.Weight;

                fish.Dob = fishModel.Dob;
                fish.Origin = fishModel.Origin;
                fish.DailyFeedAmount = fishModel.DailyFeedAmount;
                fish.Gender = fishModel.Gender;
                fish.LastHealthCheck = fishModel.LastHealthCheck;
                fish.PersonalityTraits = fishModel.PersonalityTraits;
                fish.Name = fishModel.Name;
                if (user.RoleName == "MANAGER")
                {
                    fish.IsAvailableForSale = fishModel.IsAvailableForSale;
                    fish.IsDeleted = fishModel.IsDeleted;
                }
                if (fish.KoiFishImages == null) fish.KoiFishImages = [];
                foreach (var item in fish.KoiFishImages)
                {
                    item.IsDeleted = true;
                }
                foreach (var item in fishModel.ImageUrls)
                {
                    var tmp = await _unitOfWork.KoiImageRepository.GetByUrl(item);
                    if (tmp == null)
                        fish.KoiFishImages.Add(new KoiFishImage
                        {
                            ImageUrl = item
                        });
                    else
                    {
                        tmp.IsDeleted = false;
                    }
                }
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("400 - Fail saving changes!");
                return _mapper.Map<KoiFishResponseDTO>(fish);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }

        public async Task<KoiFishResponseDTO> DeleteKoiFish(int id)
        {
            var fish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
            var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (user.RoleName == "CUSTOMER" && fish.OwnerId != null && user.Id != fish.OwnerId) throw new Exception("403 - Forbiden");
            if (fish == null)
            {
                throw new Exception("404 - Fish not found!");
            }

            var isDeleted = await _unitOfWork.KoiFishRepository.SoftRemove(fish);
            if (isDeleted)
            {
                var result = _mapper.Map<KoiFishResponseDTO>(fish);
                await _unitOfWork.SaveChangeAsync();
                return result;
            }
            else
            {
                throw new Exception("500 - Failed to delete fish");
            }
        }

        public async Task<bool> UpdateConsign(int id, int consignedBy)
        {
            var item = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
            var consigner = await _unitOfWork.UserRepository.GetAccountDetailsAsync(consignedBy);
            if (consigner == null) throw new Exception("404 - Invalid Account!");
            if (item == null) throw new Exception("404 - Invalid Fish!");
            if (item.IsConsigned == false)
            {
                item.IsConsigned = true;

                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("500 - Fail Saving!");
                return true;
            }
            else
            {
                throw new Exception("400 - Fish already been Consigned!");
            }
        }

        public async Task<bool> EndConsigned(int id)
        {
            var item = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
            if (item == null) throw new Exception("404 - Invalid Fish");
            if (item.IsConsigned == true)
            {
                item.IsConsigned = false;
                if (await _unitOfWork.SaveChangeAsync() <= 0) throw new Exception("500 - Fail Saving!");
                return true;
            }
            else
            {
                throw new Exception("400 - Fish is not being Consigned!");
            }
        }
    }
}