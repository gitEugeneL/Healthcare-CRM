using Domain.Common;
using Domain.Specializations;
using Domain.Users;
using Domain.WorkSchedules;

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

    public WorkSchedule? WorkSchedule { get; private set; }
    
    private readonly List<Specialization> _specializations = [];
    public IReadOnlyList<Specialization> Specializations => _specializations.AsReadOnly();
    
    // public List<Appointment> Appointments { get; init; } = [];

    // public List<MedicalRecord> MedicalRecords { get; init; } = [];
    
    public static Doctor Create(string? description, string? education, User user)
    {
        var doctor = new Doctor
        {
            Status = DoctorStatus.Disable,
            Description = description?.Trim(),
            Education = education?.Trim(),
            User = user,
        };
        
        return doctor;
    }
    
    public void AssignWorkSchedule(WorkSchedule workSchedule)
    {
        WorkSchedule = workSchedule;
    }
    
    public void ChangeStatus(DoctorStatus status)
    {
        Status = status;
    }
    
    public void UpdateDescription(string? description)
    {
        var normalized = description?.Trim();
    
        if (Description == normalized)
            return;
    
        Description = normalized;
    }
    
    public void UpdateEducation(string education)
    {
        var normalized = education?.Trim();

        if (Education == normalized)
            return;
        
        Education = normalized;   
    }
}
