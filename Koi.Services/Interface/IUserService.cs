using Koi.Repositories.Commons;
using Koi.Repositories.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Interface
{
    public interface IUserService
    {
        Task<ApiResult<UserDetailsModel>> DeleteUser(int id);
        Task<List<UserDetailsModel>> GetAllUsers();
        Task<ApiResult<UserDetailsModel>> GetCurrentUserAsync();
        Task<UserDetailsModel> GetUserById(int id);
        Task<ApiResult<object>> LoginAsync(UserLoginModel User);
        Task<ApiResult<UserDetailsModel>> ResigerAsync(UserSignupModel UserLogin, string role);
        Task<ApiResult<UserDetailsModel>> UpdateUserAsync(int userId, UserUpdateModel userUpdateMode);
        Task<ApiResult<UserDetailsModel>> UpdateUserWithRoleAsync(int userId, UserUpdateModel userUpdateMode, string role);
    }
}