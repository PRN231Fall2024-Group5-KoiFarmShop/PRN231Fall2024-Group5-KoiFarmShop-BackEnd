using AutoMapper;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiFishDiaryController : ControllerBase
    {
        private readonly IKoiDiaryService _koiDiaryService;
        private readonly IMapper _mapper;

        public KoiFishDiaryController(
            IKoiDiaryService _koiDiaryService,
            IMapper mapper
        )
        {
            _koiDiaryService = _koiDiaryService;
            _mapper = mapper;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] KoiFishDiaryCreateDTO diary)
        {
            try
            {
                var koiFishModel = await _koiDiaryService.CreateDiary(diary);
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
        public async Task<IActionResult> Put(int id, [FromBody] KoiFishDiaryUpdateDTO data)
        {
            try
            {
                var result = await _koiDiaryService.UpdateDiary(id, data);
                return Ok(ApiResult<KoiFishDiaryCreateDTO>.Succeed(result, "Update Koi Fish Successfully!"));
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
