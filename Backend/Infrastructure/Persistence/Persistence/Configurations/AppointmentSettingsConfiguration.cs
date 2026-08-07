// using Domain.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace Persistence.Persistence.Configurations;
//
// internal class AppointmentSettingsConfiguration : IEntityTypeConfiguration<AppointmentSettings>
// {
//     public void Configure(EntityTypeBuilder<AppointmentSettings> builder)
//     {
//         /*** One to one ***/
//         builder.HasOne(settings => settings.UserDoctor)
//             .WithOne(doctor => doctor.AppointmentSettings);
//     }
// }
