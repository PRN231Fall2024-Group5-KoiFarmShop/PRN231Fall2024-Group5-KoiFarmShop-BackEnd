﻿using Koi.BusinessObjects;
using Koi.DTOs.Enums;
using Koi.DTOs.UserDTOs;
using Koi.Repositories.Commons;

using Koi.Repositories.Models.UserModels;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public UserController(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Registers a new user with the CUSTOMER role.
        /// </summary>
        /// <param name="userSignup">The user signup data.</param>
        /// <returns>A result object indicating success or failure, with additional information.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/users/register
        ///{
        /// "email": "admin@gmail.com",
        ///  "password": "123456",
        ///  "full-name": "Hoang Tien",
        ///  "dob": "2024-09-11T01:12:17.955Z",
        ///  "phone-number": "0925136908",
        /// "profile-picture-url": "string",
        ///  "address": "string"
        ///}
        /// </remarks>
        /// <response code="200">Returns a success message with user data if registration is successful.</response>
        /// <response code="400">Returns an error message if registration fails (e.g., email already exists, invalid data).</response>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserSignupModel userSignup)
        {
            try
            {
                var data = await _userService.ResigerAsync(userSignup, "CUSTOMER");
                if (data.IsSuccess)
                {
                    // var confirmationLink = Url.Action(nameof(ConfirmEmail), "users", new { email = userLogin.Email, token = data.Message }, Request.Scheme);
                    //var message = new Message(new string[] { data.Data.Email }, "Confirmation email link", confirmationLink!);
                    // await _emailService.SendEmail(message);
                    data.Message = "Register Successfuly <3";

                    //Notification
                    var notification = new Notification
                    {
                        Title = "Welcome to Koi Farm Shop",
                        Body = "Thank you for registering with us. We hope you enjoy your shopping experience.",
                        ReceiverId = data.Data.Id,
                        Type = "USER", //ALL, USER, MANAGER, STAFF, CUSTOMER
                        Url = "https://koifarmshop.netlify.app/",
                        IsRead = false,
                    };
                    await _notificationService.PushNotification(notification);

                    return Ok(data);
                }

                return BadRequest(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// ADMIN create a new user with the CUSTOMER role.
        /// </summary>
        /// <param name="userSignup">The user signup data.</param>
        /// <param name="role">Optional new role for the user (if applicable).</param>
        /// <returns>A result object indicating success or failure, with additional information.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/users/register
        ///{
        /// "email": "admin@gmail.com",
        ///  "password": "123456",
        ///  "full-name": "Hoang Tien",
        ///  "dob": "2024-09-11T01:12:17.955Z",
        ///  "phone-number": "0925136908",
        /// "profile-picture-url": "string",
        ///  "address": "string"
        ///}
        /// </remarks>
        /// <response code="200">Returns a success message with user data if registration is successful.</response>
        /// <response code="400">Returns an error message if registration fails (e.g., email already exists, invalid data).</response>

        [HttpPost()]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserSignupModel userSignup, RoleEnums role)
        {
            try
            {
                string roleName = string.IsNullOrEmpty(role.ToString()) ? role.ToString() : "CUSTOMER";

                var data = await _userService.ResigerAsync(userSignup, roleName);
                if (data.IsSuccess)
                {
                    // var confirmationLink = Url.Action(nameof(ConfirmEmail), "users", new { email = userLogin.Email, token = data.Message }, Request.Scheme);
                    //var message = new Message(new string[] { data.Data.Email }, "Confirmation email link", confirmationLink!);
                    // await _emailService.SendEmail(message);
                    data.Message = "Register Successfuly <3";
                    return Ok(data);
                }

                return BadRequest(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Updates customer account information.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userUpdatemodel">The updated user data.</param>
        /// <returns>A result object indicating success or failure.</returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     {
        ///         "FullName": "Updated Name",
        ///         "Dob": "2001-01-01",
        ///         "Gender": "Female",
        ///         "Image": "base64_encoded_image_data",
        ///         "University": "Updated University"
        ///     }
        /// </remarks>
        /// <response code="200">Returns a success message if the update is successful.</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="400">Returns an error message if the update fails (e.g., invalid data).</response>
        [Authorize]
        [HttpPut("customers/{id}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] int id, [FromBody] CustomerProfileDTO userUpdatemodel)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, userUpdatemodel);
                if (result.IsSuccess == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Admin Updates user account information.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userUpdatemodel">The updated user data.</param>
        /// <returns>A result object indicating success or failure.</returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     {
        ///         "FullName": "Updated Name",
        ///         "Dob": "2001-01-01",
        ///         "Gender": "Female",
        ///         "Image": "base64_encoded_image_data",
        ///         "University": "Updated University"
        ///     }
        /// </remarks>
        /// <response code="200">Returns a success message if the update is successful.</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="400">Returns an error message if the update fails (e.g., invalid data).</response>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] int id, [FromBody] UserUpdateModel userUpdatemodel)
        {
            try
            {
                string roleName = !string.IsNullOrEmpty(userUpdatemodel.RoleName.ToString()) ? userUpdatemodel.RoleName.ToString() : "CUSTOMER";

                var result = await _userService.UpdateUserWithRoleAsync(id, userUpdatemodel, roleName);
                if (result.IsSuccess == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a users by their ID.
        /// </summary>
        /// <param name="id">A list of user IDs to delete.</param>
        /// <returns>A result object indicating success or failure, with the list of deleted user IDs if successful.</returns>
        /// <response code="200">Returns a success message with the list of deleted user IDs.</response>
        /// <response code="404">If none of the specified users are found.</response>
        /// <response code="400">Returns an error message if the deletion fails.</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersAsync(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authenticates a user and returns an access token if successful.
        /// </summary>
        /// <param name="user">The user login credentials.</param>
        /// <remarks>
        /// Sample request body:
        ///     {
        ///         "Email": "admin@email.com",
        ///         "Password": "123456"
        ///     }
        /// </remarks>
        /// <response code="200">Returns an access token if authentication is successful.</response>
        /// <response code="401">Returns an error message if authentication fails (e.g., invalid credentials).</response>
        /// <response code="400">Returns an error message if the request is invalid.</response>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginModel user)
        {
            try
            {
                var result = await _userService.LoginAsync(user);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Gets the currently logged-in user's information.
        /// </summary>
        /// <returns>The currently logged-in user's data.</returns>
        /// <response code="200">Returns the currently logged-in user's data.</response>
        /// <response code="404">If the user is not found or not authenticated.</response>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var result = await _userService.GetCurrentUserAsync();
            if (result.IsSuccess || result.Data != null)
            {
                return Ok(result);
            }
            return Unauthorized(result);
        }

        /// <summary>
        /// Gets all users based on specified filters and pagination parameters.
        /// </summary>
        /// <param name="paginationParameter">Pagination parameters (page number, page size).</param>
        /// <param name="userFilterModel">Filters to apply (e.g., name, email).</param>
        /// <response code="200">Returns a paginated list of users and pagination metadata in the headers.</response>
        [HttpGet()] // lấy tất cả user theo paging và filter
        public async Task<IActionResult> GetAccountByFilters(
            )
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(ApiResult<List<UserDetailsModel>>.Succeed(result, "Get list users successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get users by their specific id
        /// </summary>
        /// <response code="200">Returns an existing user</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(ApiResult<UserDetailsModel>.Succeed(user, "Get User Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}