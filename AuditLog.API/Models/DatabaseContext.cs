using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuditLog.API.Models
{
    public class DatabaseContext : IdentityDbContext<UserIdentity>
    {
        //Extract the user's claim
        private readonly IHttpContextAccessor _contextAccessor;
        public DatabaseContext(DbContextOptions options, IHttpContextAccessor contextAccessor) :base(options)
        {    
            _contextAccessor = contextAccessor;
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added
                || x.State == EntityState.Modified
                || x.State == EntityState.Deleted)
                .ToList();

            foreach (var entity in modifiedEntities)
            {
                AuditLogs.Add(new AuditLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    UserId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name),
                    Action = entity.State.ToString(),
                    DateTimeStamp = DateTime.UtcNow,
                    Changes = GetChanges(entity)
                });
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();
            foreach (var prop in entity.OriginalValues.Properties)
            {
                var orgVal = entity.OriginalValues[prop];
                var curVal = entity.CurrentValues[prop];

                if (!Equals(orgVal, curVal))
                {
                    changes.AppendLine($"{prop.Name}: From '{orgVal}' to '{curVal}'");
                }
            }
            return changes.ToString();
        }
    }
}
