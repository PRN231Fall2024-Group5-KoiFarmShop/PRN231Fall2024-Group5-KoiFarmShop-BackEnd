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



        public async Task<KoiCertificateResponseDTO> CreateKoiCertificate(CreateKoiCertificateDTO dto)
        {
            try
            {
                var existingCertificates = await _unitOfWork.KoiCertificateRepository.GetAllAsync();
                //var isExist = existingCertificates.FirstOrDefault(x => x.CertificateUrl.ToLower() == dto.CertificateUrl.ToLower());
                //if(isExist != null)
                //{
                //    throw new Exception("400 - Create failed. Certificate has already existed!");
                //}
                var koiExist = await _unitOfWork.KoiFishRepository.GetByIdAsync(dto.KoiFishId);
                if (koiExist == null)
                {
                    throw new Exception("400 - Create failed. KoiFish is not exist!");
                }
                var certificate = new KoiCertificate()
                {
                    CertificateUrl = dto.CertificateUrl,
                    KoiFishId = dto.KoiFishId,
                    CertificateType = dto.CertificateType,
                    KoiFish = koiExist
                };
                var newCertificate = await _unitOfWork.KoiCertificateRepository.AddAsync(certificate);
                int check = await _unitOfWork.SaveChangeAsync();
                if (check < 0)
                {
                    throw new Exception("400 - Create failed");
                }
                var result = _mapper.Map<KoiCertificateResponseDTO>(newCertificate);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KoiCertificateResponseDTO> UpdateKoiCertificate(UpdateKoiCertificateDTO dto, int id)
        {
            try
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
                if (check == false)
                {
                    throw new Exception("400 - Update failed");
                }
                var result = _mapper.Map<KoiCertificateResponseDTO>(existingCertificate);
                await _unitOfWork.SaveChangeAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteKoiCertificate(int id)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}