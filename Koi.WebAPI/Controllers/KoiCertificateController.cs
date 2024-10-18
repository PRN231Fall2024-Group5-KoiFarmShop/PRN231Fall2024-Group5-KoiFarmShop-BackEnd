using AutoMapper;
using Koi.DTOs.KoiCertificateDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/koi-certificates")]
    [ApiController]
    public class KoiCertificateController : ControllerBase
    {
        private readonly IKoiCertificateService _koiCertificateService;
        private readonly IMapper _mapper;
        public KoiCertificateController(IKoiCertificateService koiCertificateService, IMapper mapper)
        {
            _koiCertificateService = koiCertificateService;
            _mapper = mapper;
        }


        [HttpGet("getList/{koiId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetList(int koiId)
        {
            try
            {
                var certificates = await _koiCertificateService.GetListCertificateByKoiId(koiId);


                var koiCertificateReponseDTOs = _mapper.Map<List<KoiCertificateResponseDTO>>(certificates);

                return Ok(ApiResult<List<KoiCertificateResponseDTO>>.Succeed(koiCertificateReponseDTOs, "Get list certificates successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] KoiCertificateParams certificateParams)
        {
            try
            {
                var certificates = await _koiCertificateService.GetKoiCertificates(certificateParams);


                var koiCertificateReponseDTOs = _mapper.Map<List<KoiCertificateResponseDTO>>(certificates);

                return Ok(ApiResult<List<KoiCertificateResponseDTO>>.Succeed(koiCertificateReponseDTOs, "Get list certificates successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var certificateModel = await _koiCertificateService.GetKoiCertificateById(id);
                return Ok(ApiResult<KoiCertificateResponseDTO>.Succeed(certificateModel, "Get Certificate Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Post([FromBody] CreateKoiCertificateDTO certificate)
        {
            try
            {
                var certificateModel = await _koiCertificateService.CreateKoiCertificate(certificate);
                var locationUri = $"/api/koiCertificates/{certificateModel.Id}";

                return Created(locationUri, certificateModel);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("501"))
                    return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateKoiCertificateDTO certificate)
        {
            try
            {
                var result = await _koiCertificateService.UpdateKoiCertificate(certificate, id);
                return Ok(ApiResult<KoiCertificateResponseDTO>.Succeed(result, "Update Koi Fish Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("501"))
                    return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _koiCertificateService.DeleteKoiCertificate(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete Koi Breed Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
