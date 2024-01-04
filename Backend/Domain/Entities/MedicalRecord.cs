using Domain.Common;

namespace Domain.Entities;

public class MedicalRecord : BaseAuditableEntity
{
    public required string Title { get; set; }
    public required string DoctorNote { get; set; }
    
    /*** Relations ***/
    public required UserPatient UserPatient { get; init; }
    public Guid UserPatientId { get; init; }

    public required UserDoctor UserDoctor { get; init; }
    public Guid UserDoctorId { get; init; }

    public required Appointment Appointment { get; init; }
    public Guid AppointmentId { get; init; }
}
