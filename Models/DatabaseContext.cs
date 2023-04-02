using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetYoutube.Models;

public class DatabaseContext : IdentityDbContext<User>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Video>? Videos { get; set; }
    public DbSet<Comment>? Comments { get; set; }

    public override int SaveChanges()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity entity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        entity.UpdatedAt = now;
                        break;

                    case EntityState.Modified:
                        entity.UpdatedAt = now;
                        break;
                }
            }
        }

        return base.SaveChanges();
    }
}
