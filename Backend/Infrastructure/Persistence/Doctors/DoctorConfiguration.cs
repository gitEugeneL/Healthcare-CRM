using Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Doctors;

internal class UserDoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(d => d.Description)
            .HasMaxLength(250);
        
        builder.Property(d => d.Education)
            .HasMaxLength(250);
        
        /*** One-to-one ***/
        builder.HasOne(d => d.User)
            .WithOne()
            .HasForeignKey<Doctor>(d => d.UserId);
        
        // /*** Many to many ***/
        // builder.HasMany(doctor => doctor.Specializations)
            // .WithMany(specialization => specialization.UserDoctors);
    }
}
