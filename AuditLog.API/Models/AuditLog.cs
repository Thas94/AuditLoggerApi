using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuditLog.API.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? EntityName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? Action { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateTimeStamp { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? Changes { get; set; }
    }
}
