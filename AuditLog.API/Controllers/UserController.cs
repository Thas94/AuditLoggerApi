using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuditLog.API.Models;
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
        private readonly IMapper _mapper;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(IMapper mapper, UserManager<UserIdentity> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
        {
            if (userModel is null)
                return BadRequest();
            var user = _mapper.Map<UserIdentity>(userModel);
            var result = await _userManager.CreateAsync(user, userModel.Password);
            await _userManager.AddToRoleAsync(user, userModel.Role);
            if (result.Succeeded)
                return StatusCode(201);
            else
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(new UserResponseModel { Errors = errors});
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin([FromBody] UserModel userModel)
        {
            var userM = await _userManager.FindByEmailAsync(userModel.Email);
            var roles = await _userManager.GetRolesAsync(userM);
            var sessionTimeout = Convert.ToUInt32(_configuration["Jwt:SessionTimeOutInMinutes"]);

            if (userM != null && await _userManager.CheckPasswordAsync(userM, userModel.Password))
            {
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
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Invalid credentials" });
            }
        }
    }
}
