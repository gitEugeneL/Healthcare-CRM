using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AppointmentSettingsConfiguration : IEntityTypeConfiguration<AppointmentSettings>
{
    public void Configure(EntityTypeBuilder<AppointmentSettings> builder)
    {
        builder.Property(settings => settings.Interval)
            .HasConversion<string>();
        
        /*** One to one ***/
        builder.HasOne(settings => settings.UserDoctor)
            .WithOne(doctor => doctor.AppointmentSettings);
    }
}