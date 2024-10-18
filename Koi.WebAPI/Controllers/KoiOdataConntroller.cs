using AutoMapper;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/odata")]
    [ApiController]
    public class KoiOdataConntroller : ODataController
    {
        private readonly IDietService _dietService;
        private readonly IMapper _mapper;
        private readonly IKoiBreedService _koiBreedService;
        private readonly IKoiCertificateService _koiCertificateService;
        private readonly IKoiFishService _koiFishService;
        private readonly IKoiDiaryService _koiDiaryService;

        public KoiOdataConntroller(
           IDietService dietService,
           IMapper mapper,
           IKoiBreedService koiBreedService,
           IKoiDiaryService koiDiaryService,
           IKoiFishService koiFishService,
           IKoiCertificateService koiCertificateService
        )
        {
            _dietService = dietService;
            _mapper = mapper;
            _koiBreedService = koiBreedService;
            _koiCertificateService = koiCertificateService;
            _koiDiaryService = koiDiaryService;
            _koiFishService = koiFishService;
        }

        [HttpGet("koi-diaries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetDiaries()
        {
            try
            {
                var diaries = _koiDiaryService.GetDiaryList();
                return Ok(diaries);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }
        [HttpGet("koi-fishes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetFishes()
        {
            try
            {
                var fishes = _koiFishService.GetKoiFishes().AsQueryable();
                return Ok(fishes);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }
        [HttpGet("koi-certificates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetCertificates()
        {
            try
            {
                var certificates = _koiCertificateService.GetKoiCertificates();
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/<KoiBreedController>
        /// <summary>
        /// Get list koi breeds
        /// </summary>
        /// <returns>A list of Koi Breeds</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /KoiBreeds
        ///
        /// </remarks>
        [HttpGet("diets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetDiets()
        {
            try
            {
                var diets = _dietService.GetDiets();
                return Ok(diets);
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

        // GET: api/<KoiBreedController>
        /// <summary>
        /// Get list koi breeds
        /// </summary>
        /// <returns>A list of Koi Breeds</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /KoiBreeds
        ///
        /// </remarks>
        [HttpGet("koi-breeds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetBreeds()
        {
            try
            {
                var breeds = _koiBreedService.GetKoiBreeds();
                return Ok(breeds);
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
    }
}
