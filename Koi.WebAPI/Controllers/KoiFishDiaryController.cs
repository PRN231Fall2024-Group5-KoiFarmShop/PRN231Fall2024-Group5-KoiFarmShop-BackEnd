using AutoMapper;
using Koi.DTOs.KoiDiaryDTOs;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/odata/koi-diaries")]
    [ApiController]
    public class KoiFishDiaryController : ODataController
    {
        private readonly IKoiDiaryService _koiDiaryService;
        private readonly IMapper _mapper;

        public KoiFishDiaryController(
            IKoiDiaryService koiDiaryService,
            IMapper mapper
        )
        {
            _koiDiaryService = koiDiaryService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [EnableQuery]
        public IActionResult Get()
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
