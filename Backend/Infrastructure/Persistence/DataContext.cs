using System.Reflection;
using Domain.Abstractions;
using Domain.Common;
using Domain.Offices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

internal sealed class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Office> Offices { get; set; }
    
    // internal DbSet<MedicalRecord> MedicalRecords { get; set; }
    
    // internal DbSet<Appointment> Appointments { get; set; }
    
    // internal DbSet<AppointmentSettings> AppointmentSettings { get; set; }
    
    // internal DbSet<Specialization> Specializations { get; set; }
    
    // internal DbSet<RefreshToken> RefreshTokens { get; set; }
    
    // internal DbSet<User> Users { get; set; }
    
    // internal DbSet<UserDoctor> UserDoctors { get; set; }
    
    // internal DbSet<UserManager> UserManagers { get; set; }
    
    // internal DbSet<UserPatient> UserPatients { get; set; }
    
    // internal DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
    
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.Update();
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, ct);
    }
}
