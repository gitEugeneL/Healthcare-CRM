using Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Patients;

internal sealed class UserPatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasIndex(p => p.UserId)
            .IsUnique();
        
        builder.HasIndex(p => p.AddressId)
            .IsUnique();

        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        
        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(p => p.Pesel)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.Insurance)
          .HasMaxLength(250);

        /*** One-to-one: User ***/
        builder.HasOne(p => p.User)
            .WithOne()
            .IsRequired()
            .HasForeignKey<Patient>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*** One-to-one: Address ***/
        builder.HasOne(p => p.Address)
            .WithOne()
            .IsRequired()
            .HasForeignKey<Patient>(p => p.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Metadata
            .FindNavigation(nameof(Patient.Appointments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}