using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.Property(mr => mr.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(mr => mr.DoctorNote)
            .IsRequired();
        
        builder.Property(doctor => doctor.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); 
        
        /*** One to one ***/
        builder.HasOne(mr => mr.Appointment)
            .WithOne(a => a.MedicalRecord);
        
        /*** One to many ***/
        builder.HasOne(mr => mr.UserPatient)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(mr => mr.UserPatientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        /*** One to many ***/
        builder.HasOne(mr => mr.UserDoctor)
            .WithMany(d => d.MedicalRecords)
            .HasForeignKey(mr => mr.UserDoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
