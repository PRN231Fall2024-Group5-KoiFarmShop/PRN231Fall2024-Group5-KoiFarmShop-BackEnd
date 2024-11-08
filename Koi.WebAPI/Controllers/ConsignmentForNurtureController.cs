using Koi.DTOs.ConsignmentDTOs;
using Koi.Services.Interface;
using Koi.Repositories.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Koi.Repositories.Interfaces;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class ConsignmentForNurtureController : ControllerBase
    {
        private readonly IConsignmentForNurtureService _consignmentService;
        private readonly IClaimsService _claimsService;

        public ConsignmentForNurtureController(IConsignmentForNurtureService consignmentService, IClaimsService claimsService)
        {
            _consignmentService = consignmentService;
            _claimsService = claimsService;
        }

        /// <summary>
        /// Create a new consignment for nurturing a Koi Fish.
        /// </summary>
        /// <param name="consignmentRequestDTO">Consignment details such as KoiFishId, DietId, StartDate, and EndDate.</param>
        /// <returns>A newly created consignment if successful.</returns>
        /// <response code="200">Returns the newly created consignment.</response>
        /// <response code="400">If input validation fails or the consignment creation fails.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("nurture-consignments")]
        public async Task<IActionResult> CreateConsignment([FromBody] ConsignmentRequestDTO consignmentRequestDTO)
        {
            try
            {
                var result = await _consignmentService.CreateConsignmentAsync(consignmentRequestDTO);
                return Ok(ApiResult<ConsignmentForNurtureDTO>.Succeed(result, "Consignment created successfully."));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Get a consignment by its ID.
        /// </summary>
        /// <param name="id">The ID of the consignment.</param>
        /// <returns>The consignment if found.</returns>
        /// <response code="200">Returns the consignment if found.</response>
        /// <response code="404">If the consignment is not found.</response>
        [HttpGet("nurture-consignments/{id}")]
        public async Task<IActionResult> GetConsignmentById(int id)
        {
            try
            {
                var result = await _consignmentService.GetConsignmentByIdAsync(id);
                return Ok(ApiResult<ConsignmentForNurtureDTO>.Succeed(result, "Consignment retrieved successfully."));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Get all consignments.
        /// </summary>
        /// <returns>A list of consignments.</returns>
        /// <response code="200">Returns a list of consignments.</response>
        [HttpGet("nurture-consignments")]
        public async Task<IActionResult> GetAllConsignments()
        {
            try
            {
                var result = await _consignmentService.GetAllConsignmentsAsync();
                return Ok(ApiResult<List<ConsignmentForNurtureDTO>>.Succeed(result, "List of consignments retrieved successfully."));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

       

        /// <summary>
        /// Update an existing consignment by ID and return the updated consignment.
        /// </summary>
        /// <param name="id">The ID of the consignment to update.</param>
        /// <param name="consignmentUpdateDTO">Fields to update (only the fields provided will be updated).</param>
        /// <returns>The updated consignment object if the update was successful.</returns>
        /// <response code="200">If the consignment update was successful and returns the updated object.</response>
        /// <response code="404">If the consignment was not found.</response>
        [HttpPut("nurture-consignments/{id}")]
        public async Task<IActionResult> UpdateConsignment(int id, [FromBody] ConsignmentUpdateDTO consignmentUpdateDTO)
        {
            try
            {
                var updatedConsignment = await _consignmentService.UpdateConsignmentAsync(id, consignmentUpdateDTO);
                return Ok(ApiResult<ConsignmentForNurtureDTO>.Succeed(updatedConsignment, "Consignment updated successfully."));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Handles errors and returns appropriate HTTP status codes.
        /// </summary>
        /// <param name="ex">The exception thrown.</param>
        /// <returns>A formatted error response.</returns>
        private IActionResult HandleError(Exception ex)
        {
            if (ex.Message.Contains("400"))
                return BadRequest(ApiResult<object>.Fail(ex));
            if (ex.Message.Contains("404"))
                return NotFound(ApiResult<object>.Fail(ex));
            if (ex.Message.Contains("401"))
                return Unauthorized(ApiResult<object>.Fail(ex));
            if (ex.Message.Contains("403"))
                return StatusCode(StatusCodes.Status403Forbidden, ApiResult<object>.Fail(ex));

            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex));
        }
    }
}