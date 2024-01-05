using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        /*** One to many ***/
        builder.HasOne(appointment => appointment.UserPatient)
            .WithMany(patient => patient.Appointments)
            .HasForeignKey(appointment => appointment.UserPatientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        /*** One to many ***/
        builder.HasOne(appointment => appointment.UserDoctor)
            .WithMany(doctor => doctor.Appointments)
            .HasForeignKey(appointment => appointment.UserDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(doctor => doctor.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); 
    }
}
