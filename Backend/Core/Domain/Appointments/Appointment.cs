using Domain.Abstractions.Errors;
using Domain.Common;
using Domain.Doctors;
using Domain.Patients;

namespace Domain.Appointments;

public sealed class Appointment : BaseAuditableEntity
{
    private Appointment() { }
    
    public DateOnly Date { get; private init; }
    public TimeOnly StartTime { get; private init; }
    public TimeOnly EndTime { get; private init; }
    public AppointmentStatus Status { get; private set; }
    
    /*** Relations ***/
    public Patient Patient { get; private init; } = null!;
    public Guid PatientId { get; private init; }

    public Doctor Doctor { get; private init; } = null!;
    public Guid DoctorId { get; private init; }
    
    public static Result<Appointment> Create(
        DateOnly date, 
        TimeOnly startTime, 
        TimeOnly endTime, 
        Guid doctorId, 
        Guid patientId)
    {
        if (startTime >= endTime)
        {
            return Result.Failure<Appointment>(AppointmentErrors.EndTimeBeforeStartTime);
        }

        var appointment = new Appointment
        {
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            DoctorId = doctorId,
            PatientId = patientId,
        };
        
        return appointment;
    }

    
    private Result EnsureNotFinalized()
    {
        if (Status == AppointmentStatus.Canceled)
            return Result.Failure(AppointmentErrors.AlreadyCanceled);

        if (Status == AppointmentStatus.Completed)
            return Result.Failure(AppointmentErrors.AlreadyCompleted);

        return Result.Success();
    }

    public Result Cancel()
    {
        var check = EnsureNotFinalized();
        if (check.IsFailure)
            return check;

        Status = AppointmentStatus.Canceled;
        return Result.Success();
    }

    public Result Confirm()
    {
        var check = EnsureNotFinalized();
        if (check.IsFailure) 
            return check;

        if (Status == AppointmentStatus.Confirmed)
            return Result.Failure(AppointmentErrors.AlreadyConfirmed);

        Status = AppointmentStatus.Confirmed;
        return Result.Success();
    }

    public Result Start()
    {
        var check = EnsureNotFinalized();
        if (check.IsFailure) 
            return check;

        if (Status == AppointmentStatus.InProgress)
            return Result.Failure(AppointmentErrors.AlreadyStarted);

        if (StartTime < TimeOnly.FromDateTime(DateTime.UtcNow))
            return Result.Failure(AppointmentErrors.StartTimeTooEarly);
        
        Status = AppointmentStatus.InProgress;
        return Result.Success();
    }

    public Result Complete()
    {
        var check = EnsureNotFinalized();
        if (check.IsFailure) 
            return check;

        if (StartTime < TimeOnly.FromDateTime(DateTime.UtcNow))
            return Result.Failure(AppointmentErrors.StartTimeTooEarly);
        
        Status = AppointmentStatus.Completed;
        return Result.Success();
    }

    public Result Initialize()
    {
        var check = EnsureNotFinalized();
        if (check.IsFailure) 
            return check;
        
        Status = AppointmentStatus.Planned;
        return Result.Success();       
    }
}
