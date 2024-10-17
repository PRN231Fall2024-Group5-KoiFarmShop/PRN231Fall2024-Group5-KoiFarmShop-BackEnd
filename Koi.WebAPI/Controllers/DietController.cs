using AutoMapper;
using Koi.DTOs.DietDTOs;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/odata/diets")]
    [ApiController]
    public class DietController : ODataController
    {
        private readonly IDietService _dietService;
        private readonly IMapper _mapper;

        public DietController(
           IDietService dietService,
           IMapper mapper
        )
        {
            _dietService = dietService;
            _mapper = mapper;
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
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult Get()
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
        [HttpGet("old")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string? searchTerm)
        {
            try
            {
                var diets = await _dietService.GetDiets(searchTerm);
                //Response.AddPaginationHeader(breeds.MetaData);
                return Ok(ApiResult<List<DietCreateDTO>>.Succeed(diets, "Get list diets successfully"));
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
                var diet = await _dietService.GetDietById(id);
                return Ok(ApiResult<DietCreateDTO>.Succeed(diet, "Get diet Successfully!"));
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
        public async Task<IActionResult> Post([FromBody] DietCreateDTO model)
        {
            try
            {
                var result = await _dietService.CreateDiet(model);
                return StatusCode(StatusCodes.Status201Created, ApiResult<object>.Succeed(result, "Created!"));
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
        public async Task<IActionResult> Put(int id, [FromBody] DietCreateDTO data)
        {
            try
            {
                var result = await _dietService.UpdateDiet(id, data);
                return Ok(ApiResult<DietCreateDTO>.Succeed(result, "Update Diet Successfully!"));
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
                var result = await _dietService.DeleteDiet(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete diet Successfully!"));
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
