using Domain.Doctors;
using Domain.WorkSchedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.WorkSchedules;

internal sealed class WorkScheduleConfiguration : IEntityTypeConfiguration<WorkSchedule>
{
    public void Configure(EntityTypeBuilder<WorkSchedule> builder)
    {
        builder.HasIndex(d => d.DoctorId)
            .IsUnique();
        
        builder.Property(ws => ws.StartTime)
            .IsRequired();

        builder.Property(ws => ws.EndTime)
            .IsRequired();
        
        builder.Property(ws => ws.AppointmentDuration)
            .IsRequired()
            .HasConversion<string>();
        
        builder.PrimitiveCollection(ws => ws.Workdays)
            .ElementType(e => e.HasConversion<string>());

        builder.Metadata
            .FindProperty(nameof(WorkSchedule.Workdays))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        /*** One-to-one: Doctor ***/
        builder.HasOne<Doctor>()
            .WithOne(d => d.WorkSchedule)
            .HasForeignKey<WorkSchedule>(ws => ws.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

