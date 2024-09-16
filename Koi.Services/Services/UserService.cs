using AutoMapper;
using Koi.Repositories.Commons;
using Koi.Repositories.Interfaces;
using Koi.Repositories.Models.UserModels;
using Koi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //  private readonly IRedisService _redisService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper
            //, IRedisService redisService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            // _redisService = redisService;
        }

        public async Task<List<UserDetailsModel>> GetAllUsers()
        {
            //var Users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            //var result = new List<UserDetailsModel>();
            //foreach (var User in Users)
            //{
            //    var roleName = await _unitOfWork.UserRepository.GetRoleName(User);
            //    var lmao = _mapper.Map<UserDetailsModel>(User);
            //    lmao.RoleName = roleName;
            //    result.Add(lmao);
            //}

            var result = await _unitOfWork.UserRepository.GetUsersAsync();

            return _mapper.Map<List<UserDetailsModel>>(result);
        }

        public async Task<UserDetailsModel> GetUserById(int id)
        {
            var existingUser = await _unitOfWork.UserRepository.GetAccountDetailsAsync(id);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            var result = _mapper.Map<UserDetailsModel>(existingUser);
            return result;
        }

        public async Task<ApiResult<UserDetailsModel>> ResigerAsync(UserSignupModel UserLogin, string role)
        {
            var result = await _unitOfWork.UserRepository.AddUser(UserLogin, role);
            if (result == null)
            {
                return new ApiResult<UserDetailsModel>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "User have been existed"
                };
            }

            //  var token = await _unitOfWork.UserRepository.GenerateEmailConfirmationToken(result);

            return new ApiResult<UserDetailsModel>
            {
                Data = _mapper.Map<UserDetailsModel>(result),
                IsSuccess = true,
                Message = "Register Successfuly"
            };
        }

        public async Task<ApiResult<UserDetailsModel>> UpdateUserAsync(int userId, UserUpdateModel userUpdateMode)
        {
            var existingUser = await _unitOfWork.UserRepository.GetAccountDetailsAsync(userId);
            if (existingUser != null)
            {
                existingUser = _mapper.Map(userUpdateMode, existingUser);
                var updatedAccount = await _unitOfWork.UserRepository.UpdateAccountAsync(existingUser);

                if (updatedAccount != null)
                {
                    var response = new ApiResult<UserDetailsModel>();
                    response.Data = _mapper.Map<UserDetailsModel>(existingUser);
                    response.Message = "Updated user successfully";
                    response.IsSuccess = true;
                    return response;
                }
            }

            return new ApiResult<UserDetailsModel>
            {
                Data = null,
                IsSuccess = false,
                Message = "This user is not existed"
            };
        }

        public async Task<ApiResult<UserDetailsModel>> UpdateUserWithRoleAsync(int userId, UserUpdateModel userUpdateMode, string role)
        {
            var existingUser = await _unitOfWork.UserRepository.GetAccountDetailsAsync(userId);
            if (existingUser != null)
            {
                existingUser = _mapper.Map(userUpdateMode, existingUser);
                var updatedAccount = await _unitOfWork.UserRepository.UpdateAccountAsync(existingUser);

                if (!string.IsNullOrEmpty(role))
                {
                    await _unitOfWork.UserRepository.UpdateUserRoleAsync(userId, role);
                }

                if (updatedAccount != null)
                {
                    var response = new ApiResult<UserDetailsModel>();
                    response.Data = _mapper.Map<UserDetailsModel>(existingUser);
                    response.Message = "Updated user successfully";
                    response.IsSuccess = true;
                    return response;
                }
            }

            return new ApiResult<UserDetailsModel>
            {
                Data = null,
                IsSuccess = false,
                Message = "This user is not existed"
            };
        }

        public async Task<ApiResult<UserDetailsModel>> GetCurrentUserAsync()
        {
            var result = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (result != null)
            {
                //var role = await _unitOfWork.UserRepository.GetRole(result);
                var data = _mapper.Map<UserDetailsModel>(result);
                // data.Role = role;
                return ApiResult<UserDetailsModel>.Succeed(data, "This is current user");
            }

            return ApiResult<UserDetailsModel>.Error(null, "User is not found due to error or expiration token");
        }

        public async Task<ApiResult<object>> LoginAsync(UserLoginModel User)
        {
            var result = await _unitOfWork.UserRepository.LoginByEmailAndPassword(User);
            if (result == null)
            {
                return ApiResult<object>.Error(User.Email, "This email does not exist, please sign up for an account.");
            }
            return ApiResult<object>.Succeed(result, "Login successfully");
        }

        public async Task<ApiResult<UserDetailsModel>> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAccountDetailsAsync(id);

            if (user != null)
            {
                user = await _unitOfWork.UserRepository.SoftRemoveUserAsync(id);
                //save changes
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ApiResult<UserDetailsModel>()
                    {
                        IsSuccess = true,
                        Message = "Package " + id + " Removed successfully",
                        Data = _mapper.Map<UserDetailsModel>(user)
                    };
                }
            }
            return new ApiResult<UserDetailsModel>()
            {
                IsSuccess = false,
                Message = "There are no existed user id: " + id,
                Data = null
            };
        }
    }
}