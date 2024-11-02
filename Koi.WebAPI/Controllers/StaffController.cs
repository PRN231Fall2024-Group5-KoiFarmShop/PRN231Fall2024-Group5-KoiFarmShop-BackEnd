using Koi.BusinessObjects;
using Koi.DTOs.ConsignmentDTOs;
using Koi.DTOs.Enums;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IClaimsService _claimsService;

        public StaffController(IStaffService staffService, IClaimsService claimsService)
        {
            _staffService = staffService;
            _claimsService = claimsService;
        }

        /// <summary>
        /// Assigns or reassigns a staff member to a specific order detail.
        /// </summary>
        /// <param name="orderDetailId">The ID of the order detail to which the staff is assigned.</param>
        /// <param name="id">The ID of the staff member to assign.</param>
        /// <returns>A result indicating success or failure, with additional information.</returns>
        /// <response code="200">If the staff is successfully assigned to the order detail.</response>
        /// <response code="400">If the order status or detail status is invalid.</response>
        /// <response code="404">If the specified order detail or order is not found.</response>
        [HttpPut("staffs/{id}/task-assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignStaffToOrderDetail(int id, [FromBody] int orderDetailId)
        {
            try
            {
                var result = await _staffService.AssignStaffOrderDetail(orderDetailId, id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
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

        /// <summary>
        /// Get consignments assigned to a specific staff member.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>List of assigned consignments.</returns>
        /// <response code="200">Returns a list of consignments assigned to the staff member.</response>
        [HttpGet("staffs/{id}/nurture-consignments")]
        public async Task<IActionResult> GetAssignedConsignments(int id)
        {
            var result = await _staffService.GetAssignedConsigntment(id);
            return Ok(ApiResult<List<ConsignmentForNurtureDetailDTO>>.Succeed(result, "Assigned consignments retrieved successfully."));
        }

        /// <summary>
        /// Get order details assigned to a specific staff member.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>List of assigned order details.</returns>
        /// <response code="200">Returns a list of order details assigned to the staff member.</response>
        [HttpGet("staffs/{id}/order-details")]
        public async Task<IActionResult> GetAssignedOrderDetails(int id)
        {
            var result = await _staffService.OrderDetailDTO(id);
            return Ok(ApiResult<List<OrderDetailDTO>>.Succeed(result, "Assigned order details retrieved successfully."));
        }

        /// <summary>
        /// Update the status of a consignment to a new status.
        /// </summary>
        /// <param name="consignmentId">The ID of the consignment to update.</param>
        /// <param name="newStatus">The new status to set for the consignment.</param>
        /// <returns>The updated consignment details.</returns>
        /// <response code="200">Consignment status updated successfully.</response>
        /// <response code="404">If the consignment is not found.</response>
        [HttpPut("nurture-consignments/{consignmentId}/status")]
        public async Task<IActionResult> UpdateConsignmentStatus(int consignmentId, [FromQuery] ConsignmentStatusEnums newStatus)
        {
            try
            {
                var result = await _staffService.UpdateConsignmentStatusOnlyAsync(consignmentId, newStatus);
                return Ok(ApiResult<ConsignmentForNurtureDTO>.Succeed(result, "Consignment status updated successfully."));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Change the status of an order detail to Completed.
        /// </summary>
        /// <param name="id">The ID of the order detail.</param>
        /// <returns>The updated order with the new status.</returns>
        /// <response code="200">Order detail status updated to Completed.</response>
        /// <response code="404">If the order detail or order is not found.</response>
        [HttpPut("order-details/{id}/complete")]
        public async Task<IActionResult> ChangeToCompleted(int id)
        {
            try
            {
                var result = await _staffService.ChangeToCompleted(id);
                return Ok(ApiResult<OrderDetailDTO>.Succeed(result, "Order detail status changed to Completed."));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Change the status of an order detail to Consigned.
        /// </summary>
        /// <param name="id">The ID of the order detail.</param>
        /// <returns>The updated order with the new status.</returns>
        /// <response code="200">Order detail status updated to Consigned.</response>
        /// <response code="404">If the order detail or order is not found.</response>
        [HttpPut("order-details/{id}/nurture")]
        public async Task<IActionResult> ChangeToConsigned(int id)
        {
            try
            {
                var result = await _staffService.ChangeToConsigned(id);// change to nurturing
                return Ok(ApiResult<OrderDetailDTO>.Succeed(result, "Order detail status changed to Consigned."));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Change the status of an order detail to Shipping.
        /// </summary>
        /// <param name="id">The ID of the order detail.</param>
        /// <returns>The updated order with the new status.</returns>
        /// <response code="200">Order detail status updated to Shipping.</response>
        /// <response code="404">If the order detail or order is not found.</response>
        [HttpPut("order-details/{id}/ship")]
        public async Task<IActionResult> ChangeToShipping(int id)
        {
            try
            {
                var result = await _staffService.ChangeToShipping(id);
                return Ok(ApiResult<OrderDetailDTO>.Succeed(result, "Order detail status changed to Shipping."));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Change the status of an order detail to Shipping.
        /// </summary>
        /// <param name="id">The ID of the order detail.</param>
        /// <returns>The updated order with the new status.</returns>
        /// <response code="200">Order detail status updated to Shipping.</response>
        /// <response code="404">If the order detail or order is not found.</response>
        [HttpPut("order-details/{id}/get-fish")]
        public async Task<IActionResult> ChangeTogettingfish(int id)
        {
            try
            {
                var result = await _staffService.ChangeToGettingFish(id);
                return Ok(ApiResult<OrderDetailDTO>.Succeed(result, "Order detail status changed to Shipping."));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    return NotFound(ApiResult<object>.Fail(ex));
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}