namespace AuditLog.API.Models
{
    public class UserResponseModel
    {
        public bool? IsSuccess { get; set; }
        public List<string>? Errors { get; set; }
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Roles { get; set; }

    }
}
