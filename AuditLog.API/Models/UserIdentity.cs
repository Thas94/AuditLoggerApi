using Microsoft.AspNetCore.Identity;

namespace AuditLog.API.Models
{
    public class UserIdentity : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
