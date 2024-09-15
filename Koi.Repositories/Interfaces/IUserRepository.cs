using Koi.BusinessObjects;
using Koi.Repositories.Models.AuthenticationModels;
using Koi.Repositories.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUser(UserSignupModel newUser, string role);

        Task<User> GetAccountDetailsAsync(int userId);

        Task<List<Role>> GetAllRoleAsync();

        Task<User> GetCurrentUserAsync();

        Task<List<string>> GetRoleName(User User);

        Task<User> GetUserByEmailAsync(string email);

        Task<List<User>> GetUsersAsync();

        Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel User);

        Task<User> SoftRemoveUserAsync(int id);

        Task<User> UpdateAccountAsync(User user);

        Task<bool> UpdateUserRoleAsync(int userId, string newRole);
    }
}