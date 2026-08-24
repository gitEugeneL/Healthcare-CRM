using Domain.WorkSchedules;

namespace Application.UseCases.WorkSchedules;

public sealed record WorkScheduleResponse(
    Guid DoctorId,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int AppointmentDuration,
    string[] Workdays)
{
    public static WorkScheduleResponse FromWorkSchedule(WorkSchedule workSchedule)
    {
        return new WorkScheduleResponse(
            DoctorId: workSchedule.DoctorId,
            StartTime: workSchedule.StartTime,
            EndTime: workSchedule.EndTime,
            AppointmentDuration: (int)workSchedule.AppointmentDuration,
            Workdays: workSchedule.Workdays.Select(w => w.ToString()).ToArray());
    }
}