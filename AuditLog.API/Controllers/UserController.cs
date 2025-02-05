using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuditLog.API.Models;
using AuditLog.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuditLog.API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<UserResponseModel> CreateUser([FromBody] UserRegistrationModel userModel) => await _userService.CreateUser(userModel);
        

        [HttpPost]
        public async Task<UserResponseModel> UserLoginn([FromBody] UserLoginModel userModel) => await _userService.UserLogin(userModel);
        
    }
}
