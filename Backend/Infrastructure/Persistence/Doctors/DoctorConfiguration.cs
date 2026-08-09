using Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Doctors;

internal class UserDoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasIndex(d => d.UserId)
            .IsUnique();
        
        builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(d => d.Description)
            .HasMaxLength(250);
        
        builder.Property(d => d.Education)
            .HasMaxLength(250);
        
        /*** One-to-one: User ***/
        builder.HasOne(d => d.User)
            .WithOne()
            .IsRequired()
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*** Many-to-many ***/
        builder.HasMany(d => d.Specializations)
            .WithMany(s => s.Doctors);
        
        builder.Metadata
            .FindSkipNavigation(nameof(Doctor.Specializations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
