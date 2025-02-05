namespace AuditLog.API.Models
{
    public class UserResponseModel
    {
        public bool? IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
