using Domain.MedicalRecords;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.MedicalRecords;

internal sealed class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.HasIndex(mr => mr.AppointmentId);
        
        builder.Property(mr => mr.Title)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(mr => mr.DoctorNote)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(mr => mr.RecommendationForPatient)
            .HasMaxLength(250);
        
        builder.Property(mr => mr.Diagnosis)
            .HasMaxLength(150);
        
        builder.Property(mr => mr.IcdCode)
            .HasMaxLength(20);
            
        /*** One-to-one: Appointment ***/
        builder.HasOne(mr => mr.Appointment)
            .WithOne()
            .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
