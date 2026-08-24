using Domain.Abstractions.Errors;
using Domain.Common;

namespace Domain.WorkSchedules;

public sealed class WorkSchedule : BaseEntity
{
    private WorkSchedule() { }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public AppointmentDuration AppointmentDuration { get; private set; }
    
    private readonly List<DayOfWeek> _workdays = [];
    public IReadOnlyList<DayOfWeek> Workdays => _workdays.AsReadOnly();
    
    /*** Relations ***/
    public Guid DoctorId { get; private set; }
    
    public static Result<WorkSchedule> Create(
        Guid doctorId,
        TimeOnly startTime,
        TimeOnly endTime,
        AppointmentDuration appointmentDuration,
        List<DayOfWeek> workdays)
    {
        if (startTime >= endTime)
            return Result.Failure<WorkSchedule>(WorkScheduleErrors.InvalidTimeRange);
        
        if (workdays.Count == 0)
            return Result.Failure<WorkSchedule>(WorkScheduleErrors.EmptyWorkdays);
    
        if (workdays.Distinct().Count() != workdays.Count)
            return Result.Failure<WorkSchedule>(WorkScheduleErrors.DuplicateWorkdays);
        
        var workSchedule = new WorkSchedule
        {
            DoctorId = doctorId,
            StartTime = startTime,
            EndTime = endTime,
            AppointmentDuration = appointmentDuration
        };
        workSchedule._workdays.AddRange(workdays);
        
        return workSchedule;
    }

    public Result Update(
        TimeOnly startTime,
        TimeOnly endTime,
        AppointmentDuration appointmentDuration,
        List<DayOfWeek> workdays)
    {
        if (startTime >= endTime)
            return Result.Failure(WorkScheduleErrors.InvalidTimeRange);
        
        if (workdays.Count == 0)
            return Result.Failure(WorkScheduleErrors.EmptyWorkdays);
    
        if (workdays.Distinct().Count() != workdays.Count)
            return Result.Failure(WorkScheduleErrors.DuplicateWorkdays);

        StartTime = startTime;
        EndTime = endTime;
        AppointmentDuration = appointmentDuration;
        
        _workdays.Clear();
        _workdays.AddRange(workdays);
        
        return Result.Success();
    }
    
    public Result UpdateWorkdays(List<DayOfWeek> workdays)
    {
        if (workdays.Count == 0)
            return Result.Failure(WorkScheduleErrors.EmptyWorkdays);

        if (workdays.Distinct().Count() != workdays.Count)
            return Result.Failure(WorkScheduleErrors.DuplicateWorkdays);
        
        _workdays.Clear();
        _workdays.AddRange(workdays);
        
        return Result.Success();
    }

    public bool IsAvailableDay(DayOfWeek dayOfWeek)
    {
        return Workdays.Contains(dayOfWeek);
    }
}


