using Domain.Common;
using Domain.Specializations;
using Domain.Users;

namespace Domain.Doctors;

public sealed class Doctor : BaseAuditableEntity
{
    private Doctor() { }
    
    public DoctorStatus Status { get; private set; }
    public string? Description { get; private set; }
    public string? Education { get; private set; }
    
    /*** Relations ***/
    public User User { get; private init; } = null!;
    public Guid UserId { get; private init; }
    
    // public required AppointmentSettings AppointmentSettings { get; init; }
    // public Guid AppointmentSettingsId { get; init; }
    
    public List<Specialization> Specializations { get; private init; } = [];

    // public List<Appointment> Appointments { get; init; } = [];

    // public List<MedicalRecord> MedicalRecords { get; init; } = [];
    
    public static Doctor Create(string? description, string? education, User user)
    {
        var doctor = new Doctor
        {
            Status = DoctorStatus.Active,
            Description = description,
            Education = education,
            User = user
        };
        
        return doctor;
    }
}
