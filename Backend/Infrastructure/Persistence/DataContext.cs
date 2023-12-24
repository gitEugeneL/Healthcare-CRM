using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public required DbSet<Specialization> Specializations { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<UserDoctor> UserDoctors { get; set; }
    public required DbSet<UserManager> UserManagers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfiguration(new SpecializationConfiguration())
            .ApplyConfiguration(new RefreshTokenConfiguration())
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new UserDoctorConfiguration())
            .ApplyConfiguration(new UserManagerConfiguration());
        
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
