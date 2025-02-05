using System.ComponentModel.DataAnnotations;

namespace AuditLog.API.Models
{
    public class UserLoginModel
    {

        [Required(ErrorMessage = "User name is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        
    }
}
