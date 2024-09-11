using AutoMapper;
using Koi.BusinessObjects.DTO.KoiFishDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiFishController : ControllerBase
    {
        private readonly IKoiFishService _koiFishService;
        private readonly IMapper _mapper;

        public KoiFishController(
            IKoiFishService koiFishService,
            IMapper mapper
        )
        {
            _koiFishService = koiFishService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] KoiParams koiFishParams)
        {
            try
            {
                var breeds = await _koiFishService.GetKoiFishes(koiFishParams);

                //Response.AddPaginationHeader(breeds.MetaData);

                var koiFishReponseDTOs = _mapper.Map<List<KoiFishResponseDTO>>(breeds);

                return Ok(ApiResult<List<KoiFishResponseDTO>>.Succeed(koiFishReponseDTOs, "Get list koi fishes successfully"));
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
                var fishModel = await _koiFishService.GetKoiFishById(id);
                return Ok(ApiResult<KoiFishResponseDTO>.Succeed(fishModel, "Get Fish Successfully!"));
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

        // POST api/<KoiBreedController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Post([FromBody] CreateKoiFishDTO fish)
        {
            try
            {
                var koiFishModel = await _koiFishService.CreateKoiFish(fish);
                return Created();
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

        // PUT api/<KoiBreedController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Put(int id, [FromBody] CreateKoiFishDTO data)
        {
            try
            {
                var result = await _koiFishService.UpdateKoiFish(id, data);
                return Ok(ApiResult<KoiFishResponseDTO>.Succeed(result, "Update Koi Fish Successfully!"));
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
                var result = await _koiFishService.DeleteKoiFish(id);
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