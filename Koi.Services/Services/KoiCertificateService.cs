using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.Repositories.Helper;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;

namespace Koi.Services.Services
{
    public class KoiCertificateService : IKoiCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KoiCertificateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<KoiCertificateResponseDTO> GetKoiCertificateById(int id)
        {
            try
            {
                var certificate = await _unitOfWork.KoiCertificateRepository.GetByIdAsync(id);
                if (certificate == null)
                {
                    throw new Exception("404 - Certificate not found!");
                }
                var result = _mapper.Map<KoiCertificateResponseDTO>(certificate);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<KoiCertificateResponseDTO>> GetListCertificateByKoiId(int koiId)
        {
            try
            {
                var list = await _unitOfWork.KoiCertificateRepository.GetListCertificateByKoiIdAsync(koiId);
                if (list == null)
                {
                    throw new Exception("404 - Certificate not found!");
                }
                var result = _mapper.Map<List<KoiCertificateResponseDTO>>(list);
                result = result.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<KoiCertificate> GetKoiCertificates() => _unitOfWork.KoiCertificateRepository.GetQueryable();

        public async Task<List<KoiCertificateResponseDTO>> GetKoiCertificates(KoiCertificateParams certificateParams)
        {
            try
            {
                var list = await _unitOfWork.KoiCertificateRepository.GetAllAsync();

                if (!string.IsNullOrEmpty(certificateParams.KoiName))
                {
                    list = list
                        .Where(x => x.KoiFish.Name.ToLower().Contains(certificateParams.KoiName.ToLower()))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(certificateParams.UserName))
                {
                    list = list
                        .Where(x => x.KoiFish.Owner.FullName.ToLower().Contains(certificateParams.UserName.ToLower()))
                        .ToList();
                }

                var result = _mapper.Map<List<KoiCertificateResponseDTO>>(list);

                result = result.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<KoiCertificateResponseDTO> CreateKoiCertificate(CreateKoiCertificateDTO dto)
        {
            var existingCertificates = await _unitOfWork.KoiCertificateRepository.GetAllAsync();
            var koiExist = await _unitOfWork.KoiFishRepository.GetByIdAsync(dto.KoiFishId);

            if (koiExist == null)
            {
                throw new Exception("400 - Create failed. KoiFish does not exist!");
            }

            var certificate = new KoiCertificate()
            {
                CertificateUrl = dto.CertificateUrl,
                KoiFishId = dto.KoiFishId,
                CertificateType = dto.CertificateType,
                KoiFish = koiExist
            };

            // Add the new certificate
            var newCertificate = await _unitOfWork.KoiCertificateRepository.AddAsync(certificate);
            int result = await _unitOfWork.SaveChangeAsync();

            if (result <= 0) // Check for save failure
            {
                throw new Exception("400 - Create failed");
            }
            var res = _mapper.Map<KoiCertificateResponseDTO>(newCertificate);

            return res;
        }

        public async Task<KoiCertificateResponseDTO> UpdateKoiCertificate(UpdateKoiCertificateDTO dto, int id)
        {
            
            var existingCertificate = await _unitOfWork.KoiCertificateRepository.GetByIdAsync(id);
            if (existingCertificate == null)
            {
                throw new Exception("400 - Update failed");
            }
            if (!string.IsNullOrEmpty(dto.CertificateUrl))
            {
                existingCertificate.CertificateUrl = dto.CertificateUrl;
            }
            if (!string.IsNullOrEmpty(dto.CertificateType))
            {
                existingCertificate.CertificateType = dto.CertificateType;
            }
            var check = await _unitOfWork.KoiCertificateRepository.Update(existingCertificate);
                
            int result = await _unitOfWork.SaveChangeAsync();
            if (result <= 0) // Check for save failure
            {
                throw new Exception("400 - Update failed");
            }
            var res = _mapper.Map<KoiCertificateResponseDTO>(existingCertificate);
            return res;
           
        }

        public async Task<bool> DeleteKoiCertificate(int id)
        {
                var existingCertificate = await _unitOfWork.KoiCertificateRepository.GetByIdAsync(id);
                if (existingCertificate == null)
                {
                    throw new Exception("400 - Delete failed");
                }
                var check = await _unitOfWork.KoiCertificateRepository.SoftRemove(existingCertificate);
                if (check == false)
                {
                    throw new Exception("400 - Delete failed");
                }
                await _unitOfWork.SaveChangeAsync();
            return check;
        }
    }
}
