using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public required DbSet<Appointment> Appointments { get; init; }
    public required DbSet<AppointmentSettings> AppointmentSettings { get; init; }
    public required DbSet<Specialization> Specializations { get; init; }
    public required DbSet<RefreshToken> RefreshTokens { get; init; }
    public required DbSet<User> Users { get; init; }
    public required DbSet<UserDoctor> UserDoctors { get; init; }
    public required DbSet<UserManager> UserManagers { get; init; }
    public required DbSet<UserPatient> UserPatients { get; init; }
    public required DbSet<Address> Addresses { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfiguration(new AppointmentConfig())
            .ApplyConfiguration(new AppointmentSettingsConfiguration())
            .ApplyConfiguration(new SpecializationConfiguration())
            .ApplyConfiguration(new RefreshTokenConfiguration())
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new UserPatientConfiguration())
            .ApplyConfiguration(new UserDoctorConfiguration())
            .ApplyConfiguration(new UserManagerConfiguration())
            .ApplyConfiguration(new AddressConfiguration());

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
