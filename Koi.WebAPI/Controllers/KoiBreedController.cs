using AutoMapper;
using Koi.DTOs.KoiBreedDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/koi-breeds")]
    [ApiController]
    public class KoiBreedController : ControllerBase
    {
        private readonly IKoiBreedService _koiBreedService;
        private readonly IMapper _mapper;

        public KoiBreedController(
            IKoiBreedService koiBreedService,
            IMapper mapper
        )
        {
            _koiBreedService = koiBreedService;
            _mapper = mapper;
        }


        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] KoiBreedParams koiBreedsParams)
        {
            try
            {
                var breeds = await _koiBreedService.GetKoiBreeds(koiBreedsParams);

                //Response.AddPaginationHeader(breeds.MetaData);

                var koiFishReponseDTOs = _mapper.Map<List<KoiBreedResponseDTO>>(breeds);

                return Ok(ApiResult<List<KoiBreedResponseDTO>>.Succeed(koiFishReponseDTOs, "Get list koi breed successfully"));
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

        // GET api/<KoiBreedController>/5
        /// <summary>
        /// Get koi breed by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A koi breed With Id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /events/1
        ///     {
        ///        "id": 1,
        ///        "name": "breed #1",
        ///        "content": "breed content #1",
        ///     }
        ///
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var breedModel = await _koiBreedService.GetKoiBreedById(id);
                return Ok(ApiResult<KoiBreedResponseDTO>.Succeed(breedModel, "Get breed Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // POST api/<KoiBreedController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] KoiBreedCreateDTO koiBreed)
        {
            try
            {
                var koiFishModel = await _koiBreedService.CreateKoiBreed(koiBreed);
                return StatusCode(StatusCodes.Status201Created, ApiResult<object>.Succeed(koiFishModel, "Created!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // PUT api/<KoiBreedController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] KoiBreedCreateDTO data)
        {
            try
            {
                var result = await _koiBreedService.UpdateKoiBreed(id, data);
                return Ok(ApiResult<KoiBreedResponseDTO>.Succeed(result, "Update Koi Breed Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }

        // DELETE api/<KoiBreedController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _koiBreedService.DeleteKoiBreed(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete Koi Breed Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }
    }
}