﻿using Koi.BusinessObjects;
using Koi.Repositories.Interfaces;
using Koi.Repositories.Models.AuthenticationModels;
using Koi.Repositories.Models.UserModels;
using Koi.Repositories.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KoiFarmShopDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        private readonly IConfiguration _configuration;

        // identity collection
        private readonly UserManager<User> _userManager;

        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(KoiFarmShopDbContext context, ICurrentTime timeService, IClaimsService claimsService, IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<User> AddUser(UserSignupModel newUser, string role)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(newUser.Email);
                if (userExist != null)
                {
                    return null;
                }
                var user = new User
                {
                    Email = newUser.Email,
                    UserName = newUser.Email,
                    FullName = newUser.FullName,
                    Dob = newUser.Dob,
                    RoleName = role,
                    IsActive = false,
                    PhoneNumber = newUser.PhoneNumber,
                    ImageUrl = newUser.ImageUrl,
                    Address = newUser.Address,
                    CreatedBy = _claimsService.GetCurrentUserId,
                    CreatedDate = _timeService.GetCurrentTime()
                };

                if (newUser.FullName != null)
                {
                    user.UnsignFullName = StringTools.ConvertToUnSign(newUser.FullName);
                }

                var result = await _userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    Console.WriteLine($"New user ID: {user.Id}");

                    // Kiểm tra xem vai trò đã tồn tại chưa, nếu chưa thì tạo vai trò mới đã tôi ưu
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (!roleExists)
                    {
                        var newRole = new Role();
                        newRole.Name = role;

                        await _roleManager.CreateAsync(newRole);
                    }

                    // Nếu vai trò tồn tại, gán vai trò cho người dùng
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }

                    return user;
                }
                else
                {
                    // Tạo người dùng không thành công, xem thông tin lỗi và xử lý
                    StringBuilder errorValue = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        errorValue.Append($"{item.Description}");
                    }
                    throw new Exception(errorValue.ToString()); // bắn zề cho GlobalEx midw
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GenerateTokenForResetPassword(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel User)
        {
            var userExist = await _userManager.FindByEmailAsync(User.Email);
            if (userExist == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userExist, User.Password, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(userExist);

                var authClaims = new List<Claim> // add User vào claim
                {
                    new Claim(ClaimTypes.Name, userExist.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                //generate refresh token
                var refreshToken = TokenTools.GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                userExist.RefreshToken = refreshToken;
                userExist.RefreshTokenExpiryTime = _timeService.GetCurrentTime().AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(userExist); //update 2 jwt
                var token = GenerateJWTToken.CreateToken(authClaims, _configuration, _timeService.GetCurrentTime());
                return new ResponseLoginModel
                {
                    Status = true,
                    Message = "Login successfully",
                    JWT = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo,
                    JWTRefreshToken = refreshToken,
                    UserId = userExist.Id
                };
            }
            else
            {
                //if (!UserExist.EmailConfirmed)
                //{
                //    return new ResponseLoginModel
                //    {
                //        Status = false,
                //        Message = "Your email haven't verified yet, please check",
                //    };
                //}

                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Invalid login attempt. Please check your password."
                };
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var result = await _userManager.FindByIdAsync(id.ToString());
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<User> UpdateAccountAsync(User user)
        {
            try
            {
                user.ModifiedDate = _timeService.GetCurrentTime();
                user.ModifiedBy = _claimsService.GetCurrentUserId;
                user.UnsignFullName = StringTools.ConvertToUnSign(user.FullName);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return user;
                }
                else
                {
                    // Tạo người dùng không thành công, xem thông tin lỗi và xử lý
                    StringBuilder errorValue = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        errorValue.Append($"{item.Description}");
                    }
                    throw new Exception(errorValue.ToString()); // bắn zề cho GlobalEx midw
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetAccountDetailsAsync(int userId)
        {
            var accounts = await _userManager.Users.ToListAsync();
            var account = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public async Task<User> SoftRemoveUserAsync(int id)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new Exception("This user is not existed");
                }

                user.IsDeleted = true;
                user.DeletionDate = _timeService.GetCurrentTime();
                user.DeleteBy = _claimsService.GetCurrentUserId;
                _context.Entry(user).State = EntityState.Modified;
                // await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
        {
            // Tìm người dùng theo ID
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Lấy các vai trò hiện tại của người dùng
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Xóa các vai trò hiện tại của người dùng
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                StringBuilder errorMessages = new StringBuilder();
                foreach (var error in removeRolesResult.Errors)
                {
                    errorMessages.Append(error.Description);
                }
                throw new Exception($"Failed to remove current roles: {errorMessages}");
            }

            // Kiểm tra xem vai trò mới có tồn tại chưa
            var roleExists = await _roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
            {
                // Nếu vai trò chưa tồn tại, tạo vai trò mới
                var role = new Role();
                role.Name = newRole;
                var newRoleResult = await _roleManager.CreateAsync(role);
                if (!newRoleResult.Succeeded)
                {
                    throw new Exception("Failed to create new role.");
                }
            }

            // Thêm vai trò mới cho người dùng
            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addRoleResult.Succeeded)
            {
                StringBuilder errorMessages = new StringBuilder();
                foreach (var error in addRoleResult.Errors)
                {
                    errorMessages.Append(error.Description);
                }
                throw new Exception($"Failed to add new role: {errorMessages}");
            }

            return true;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var user = await _userManager.Users.Include(x => x.Wallet).FirstOrDefaultAsync(x => x.Id == _claimsService.GetCurrentUserId);
            if (user != null)
            {
                return user;
            }

            return null;
        }

        public async Task<List<Role>> GetAllRoleAsync()
        {
            try
            {
                // get all users
                var roles = await _roleManager.Roles.ToListAsync();
                return roles;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<string>> GetRoleName(User User)
        {
            var result = await _userManager.GetRolesAsync(User);

            return result.ToList();
        }

        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users.Include(x=>x.Roles).ToListAsync();
                var roles = users.Select(x => x.Roles.First().Name);
                var rolenames = await _roleManager.Roles.ToListAsync();

                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}