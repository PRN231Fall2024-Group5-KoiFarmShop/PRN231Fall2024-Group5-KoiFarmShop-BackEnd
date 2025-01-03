﻿using AutoMapper;
using Koi.DTOs.KoiFishDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{

    [Route("api/v1/koi-fishes")]
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




        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] KoiParams koiFishParams)
        {
            try
            {
                var breeds = await _koiFishService.GetKoiFishes(koiFishParams);
                var list = breeds.ToList();
                return Ok(new { isSuccess = true, data = _mapper.Map<List<KoiFishResponseDTO>>(list), metadata = breeds.MetaData, message = "Get Fishes Successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
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

                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }


        // POST api/<KoiBreedController>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] KoiFishCreateDTO fish)
        {
            try
            {
                var koiFishModel = await _koiFishService.CreateKoiFish(fish);
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
        public async Task<IActionResult> Put(int id, [FromBody] KoiFishUpdateDTO data)
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
                if (ex.Message.Contains("403"))
                    return Forbid();
                if (ex.Message.Contains("401"))
                    return Unauthorized();
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
                var result = await _koiFishService.DeleteKoiFish(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete Koi Breed Successfully!"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                if (ex.Message.Contains("403"))
                    return Forbid();
                if (ex.Message.Contains("401"))
                    return Unauthorized();
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
            }
        }
    }
}