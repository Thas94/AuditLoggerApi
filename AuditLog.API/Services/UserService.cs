using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuditLog.API.Models;
using AuditLog.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuditLog.API.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IMapper mapper, ILogger<UserService> logger, DatabaseContext databaseContext, UserManager<UserIdentity> userManager, IConfiguration configuration) : base(mapper, logger, databaseContext, userManager, configuration) { }
    
        public async Task<UserResponseModel> CreateUser(UserRegistrationModel userModel)
        {
            UserResponseModel userResponse = new UserResponseModel();
            try
            {
                if (userModel is null)
                    return userResponse;
                var user = _mapper.Map<UserIdentity>(userModel);
                var result = await _userManager.CreateAsync(user, userModel.Password);
                await _userManager.AddToRoleAsync(user, userModel.Role);

                if (result.Succeeded)
                {
                    var userM = await _userManager.FindByEmailAsync(userModel.Email);
                    userResponse = new UserResponseModel
                    {
                        IsSuccess = true,
                    };
                }
                else
                {
                    userResponse.Errors = result.Errors.Select(x => x.Description).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }
            return userResponse;
        }

        public async Task<UserResponseModel> UserLogin(UserLoginModel userModel)
        {
            UserResponseModel userResponse = new UserResponseModel();
            try
            {
                var userM = await _userManager.FindByEmailAsync(userModel.Email);

                if (userM != null && await _userManager.CheckPasswordAsync(userM, userModel.Password))
                {
                    var roles = await _userManager.GetRolesAsync(userM);
                    var sessionTimeout = Convert.ToUInt32(_configuration["Jwt:SessionTimeOutInMinutes"]);
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserID", userM.Id.ToString()),
                            new Claim(ClaimTypes.Role, roles.First())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    userResponse = new UserResponseModel { IsSuccess = true, Roles = roles, Token = token, UserId = userM.Id};
                }
                else
                {
                    SaveIncorrectDetails(userM, userModel);
                    userResponse.Errors = new List<string> { "Invalid credentials" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }
            return userResponse;
        }

        private void SaveIncorrectDetails(UserIdentity user, UserLoginModel userModel)
        {
            try
            {
                _databaseContext.IncorrectPasswords.Add(new IncorrectPasswords { ExpectedPassword = _databaseContext.Users.FirstOrDefault(x => x.Id == user.Id).PasswordHash, IncorrectPassword = userModel.Password, CreatedBy = $"{user.FirstName} {user.FirstName}", CreatedDate = DateTime.UtcNow, UserId = user.Id});
                _databaseContext.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message, ex.StackTrace);
            }
        }
    }
}
