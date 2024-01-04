using Domain.Common;

namespace Domain.Entities;

public class Appointment : BaseAuditableEntity
{
    public DateOnly Date { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public bool IsCompleted { get; set; }
    public bool IsCanceled { get; set; }
    
    /*** Relations ***/
    public required UserPatient UserPatient { get; init; }
    public Guid UserPatientId { get; init; }

    public required UserDoctor UserDoctor { get; init; }
    public Guid UserDoctorId { get; init; }

    public MedicalRecord? MedicalRecord { get; init; }
}
