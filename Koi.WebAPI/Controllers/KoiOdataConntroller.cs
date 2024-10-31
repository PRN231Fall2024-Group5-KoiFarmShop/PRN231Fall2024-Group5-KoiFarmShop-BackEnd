using AutoMapper;
using Koi.BusinessObjects;
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
        private readonly IKoiBreedService _koiBreedService;
        private readonly IKoiCertificateService _koiCertificateService;
        private readonly IKoiFishService _koiFishService;
        private readonly IKoiDiaryService _koiDiaryService;
        private readonly IOrderService _paymentService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRequestForSaleService _requestForSaleService;
        private readonly IBlogService _blogService;

        public KoiOdataConntroller(
           IDietService dietService,
           IMapper mapper,
           IKoiBreedService koiBreedService,
           IKoiDiaryService koiDiaryService,
           IKoiFishService koiFishService,
           IKoiCertificateService koiCertificateService,
           IOrderService paymentService,
           IUserService userService,
           IRequestForSaleService requestForSaleService,
           IBlogService blogService
        )
        {
            _userService = userService;
            _dietService = dietService;
            _koiBreedService = koiBreedService;
            _koiCertificateService = koiCertificateService;
            _koiDiaryService = koiDiaryService;
            _koiFishService = koiFishService;
            _paymentService = paymentService;
            _mapper = mapper;
            _requestForSaleService = requestForSaleService;
            _blogService = blogService;
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
        [HttpGet("my-koi-fishes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetMyFishes()
        {
            try
            {
                var fishes = _koiFishService.GetMyKoiFishes().AsQueryable();
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

        [HttpGet("request-for-sales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetRequestForSales()
        {
            try
            {
                var requests = _requestForSaleService.GetRequestForSales().AsQueryable();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("my-request-for-sales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult GetMyRequestForSales()
        {
            try
            {
                var requests = _requestForSaleService.GetMyRequestForSales().AsQueryable();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("orders")]
        [EnableQuery]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var result = await _paymentService.GetOrdersAsync();

                return Ok(_mapper.Map<List<Order>>(result).AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("accounts")]
        [EnableQuery]

        public async Task<IActionResult> GetAccountByFilters(
            )
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(_mapper.Map<List<User>>(result).AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
