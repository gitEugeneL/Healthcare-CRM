namespace Domain.WorkSchedules;

public interface IWorkScheduleRepository
{
    Task InsertWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken ct);
    
    Task<WorkSchedule?> FindWorkScheduleByDoctorIdWithTracking(Guid doctorId, CancellationToken ct);
    
    Task<WorkSchedule?> FindWorkScheduleByDoctorId(Guid doctorId, CancellationToken ct);
}
