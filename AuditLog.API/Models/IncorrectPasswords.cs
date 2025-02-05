using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuditLog.API.Models
{
    public class IncorrectPasswords
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? IncorrectPassword { get; set; }
        
        [Column(TypeName = "nvarchar(MAX)")]
        public string? ExpectedPassword { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
