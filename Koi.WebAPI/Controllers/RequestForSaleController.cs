using AutoMapper;
using Koi.BusinessObjects;
using Koi.DTOs.RequestForSaleDTOs;
using Koi.Repositories.Commons;
using Koi.Repositories.Helper;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RequestForSaleController : ControllerBase
    {
        private readonly IRequestForSaleService _requestForSaleService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        public RequestForSaleController(IRequestForSaleService requestForSaleService, IMapper mapper, INotificationService notificationService)
        {
            _requestForSaleService = requestForSaleService;
            _mapper = mapper;
            _notificationService = notificationService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] RequestForSaleParams requestForSaleParams)
        {
            try
            {
                var requestForSales = await _requestForSaleService.GetRequestForSales(requestForSaleParams);

                //Response.AddPaginationHeader(breeds.MetaData);

                var requestForSaleReponseDTOs = _mapper.Map<List<RequestForSaleResponseDTO>>(requestForSales);

                return Ok(ApiResult<List<RequestForSaleResponseDTO>>.Succeed(requestForSaleReponseDTOs, "Get list request for sales successfully"));
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
                var requestForSaleModel = await _requestForSaleService.GetRequestForSaleById(id);
                return Ok(ApiResult<RequestForSaleResponseDTO>.Succeed(requestForSaleModel, "Get request for sale successfully!"));
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] RequestForSaleCreateDTO requestForSale)
        {
            try
            {
                var requestForSaleModel = await _requestForSaleService.CreateRequestForSale(requestForSale);
                var notification = new Notification
                {
                    Title = "New Request Created",
                    Body = $"A new request with ID {requestForSaleModel.Id} has been created.",
                    ReceiverId = requestForSaleModel.UserId,
                    Type = "MANAGER",
                    Url = $"/manager/sale-request"
                };
                await _notificationService.PushNotification(notification);

                return Created(string.Empty, ApiResult<RequestForSaleResponseDTO>.Succeed(requestForSaleModel, "Create request for sale successfully!"));
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
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Put(int id, [FromBody] RequestForSaleUpdateDTO requestForSale)
        {
            try
            {
                var result = await _requestForSaleService.UpdateRequestForSale(requestForSale, id);
                return Ok(ApiResult<RequestForSaleResponseDTO>.Succeed(result, "Update request for sale successfully!"));
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
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _requestForSaleService.DeleteRequestForSale(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete request for sale successfully!"));
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
        [HttpPut("{id}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            try
            {
                var result = await _requestForSaleService.ApproveRequest(id);
                // Create a notification after approving the request
                var notification = new Notification
                {
                    Title = "Request Approved",
                    Body = $"Your request with ID {id} has been approved.",
                    ReceiverId = result.UserId, // Assuming result has UserId property or similar
                    Type = "USER",
                    Url = $"/manager/sale-request"
                };
                await _notificationService.PushNotification(notification);
                return Ok(ApiResult<RequestForSaleResponseDTO>.Succeed(result, "Request approved successfully!"));
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
        [HttpPut("{id}/reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RejectRequest(int id)
        {
            try
            {
                var result = await _requestForSaleService.RejectRequest(id);

                var notification = new Notification
                {
                    Title = "Request Rejected",
                    Body = $"Your request with ID {id} has been rejected.",
                    ReceiverId = result.UserId,
                    Type = "USER",
                    Url = $"/manager/sale-request"
                };
                await _notificationService.PushNotification(notification);

                return Ok(ApiResult<RequestForSaleResponseDTO>.Succeed(result, "Request rejected successfully!"));
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
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelRequest(int id)
        {
            try
            {
                var result = await _requestForSaleService.CancelRequest(id);

                var notification = new Notification
                {
                    Title = "Request Canceled",
                    Body = $"Your request with ID {id} has been canceled.",
                    ReceiverId = result.UserId,
                    Type = "USER",
                    Url = $"/manager/sale-request"
                };
                await _notificationService.PushNotification(notification);


                return Ok(ApiResult<RequestForSaleResponseDTO>.Succeed(result, "Request canceled successfully!"));
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
    }
}
