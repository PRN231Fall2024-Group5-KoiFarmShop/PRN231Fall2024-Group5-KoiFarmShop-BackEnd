using AutoMapper;
using Koi.DTOs.PaymentDTOs;
using Koi.Repositories.Commons;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/order-detail")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailServices _orderDetailServices;
        private readonly IMapper _mapper;

        public OrderDetailController(
            IOrderDetailServices orderDetailServices,
            IMapper mapper
        )
        {
            _orderDetailServices = orderDetailServices;
            _mapper = mapper;
        }

        [HttpPut("change-to-consigned/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeToConsigned(int id)
        {
            try
            {
                var result = await _orderDetailServices.ChangeToConsigned(id);
                return Ok(ApiResult<OrderDTO>.Succeed(_mapper.Map<OrderDTO>(result), "Order Updated!"));
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

        [HttpPut("change-to-completed/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeToCompleted(int id)
        {
            try
            {
                var result = await _orderDetailServices.ChangeToCompleted(id);
                return Ok(ApiResult<OrderDTO>.Succeed(_mapper.Map<OrderDTO>(result), "Order Updated!"));
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

        [HttpPut("change-to-shipping/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeToShipping(int id)
        {
            try
            {
                var result = await _orderDetailServices.ChangeToShipping(id);
                return Ok(ApiResult<OrderDTO>.Succeed(_mapper.Map<OrderDTO>(result), "Order Updated!"));
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

        [HttpPut("{id}/assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignStaffToOrderDetail(int id, [FromBody] int staffId)
        {
            try
            {
                var result = await _orderDetailServices.AssignStaffOrderDetail(id, staffId);
                return Ok(ApiResult<OrderDetailDTO>.Succeed(_mapper.Map<OrderDetailDTO>(result), "Order Updated!"));
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