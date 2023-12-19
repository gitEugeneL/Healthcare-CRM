using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    { 
        builder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new RefreshTokenConfiguration());
        
        base.OnModelCreating(builder);
    }
    
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken token = default)
    {
        foreach (var entity in ChangeTracker
                     .Entries()
                     .Where(x => x is { Entity: BaseAuditableEntity, State: EntityState.Modified })
                     .Select(x => x.Entity)
                     .Cast<BaseAuditableEntity>())
        {
            entity.Updated = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, token);
    }
}
