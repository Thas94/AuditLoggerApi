using AuditLog.API.Models;

namespace AuditLog.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> UserLogin(UserLoginModel userModel);
        Task<UserResponseModel> CreateUser(UserRegistrationModel userModel);
    }
}
