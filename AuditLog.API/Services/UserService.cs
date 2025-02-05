using AuditLog.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuditLog.API.Services
{
    public class UserService : BaseService
    {
        public UserService(IMapper mapper, ILogger<UserService> logger, DatabaseContext databaseContext, UserManager<UserIdentity> userManager, IConfiguration configuration) : base(mapper, logger, databaseContext, userManager, configuration) { }
    
        //public async Task<IActionResult> RegisterUser(UserModel userModel)
        //{
        //    try
        //    {
        //        if (userModel is null)
        //            return BadRequestResult();

        //        var user = _mapper.Map<UserIdentity>(userModel);
        //        var result = await _userManager.CreateAsync(user, userModel.Password);
        //        await _userManager.AddToRoleAsync(user, userModel.Role);
        //        if (result.Succeeded)
        //            return Results.Ok(result);
        //        else
        //            return Results.BadRequest(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex.StackTrace);
        //    }
        //}
    }
}
