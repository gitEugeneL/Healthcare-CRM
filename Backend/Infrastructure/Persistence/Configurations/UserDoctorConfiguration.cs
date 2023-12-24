using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserDoctorConfiguration : IEntityTypeConfiguration<UserDoctor>
{
    public void Configure(EntityTypeBuilder<UserDoctor> builder)
    {
        builder.Property(doctor => doctor.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(doctor => doctor.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        /*** One to one ***/
        builder.HasOne(doctor => doctor.User)
            .WithOne(user => user.UserDoctor);
        
        /*** Many to many ***/
        builder.HasMany(doctor => doctor.Specializations)
            .WithMany(specialization => specialization.UserDoctors);
    }
}
