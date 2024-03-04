using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserPatientConfiguration : IEntityTypeConfiguration<UserPatient>
{
    public void Configure(EntityTypeBuilder<UserPatient> builder)
    {
        builder.Property(patient => patient.Pesel)
            .IsFixedLength()
            .HasMaxLength(11);

        builder.Property(patient => patient.Insurance)
            .HasMaxLength(200);

        /*** One to one ***/
        builder.HasOne(patient => patient.User)
            .WithOne(user => user.UserPatient);
    }
}