using Domain.Abstractions.Errors;
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

    public WorkSchedule WorkSchedule { get; private set; } = null!;
    
    private readonly List<Specialization> _specializations = [];
    public IReadOnlyList<Specialization> Specializations => _specializations.AsReadOnly();
    
    
    // public List<Appointment> Appointments { get; init; } = [];

    // public List<MedicalRecord> MedicalRecords { get; init; } = [];
    
    public static Doctor Create(string? description, string? education, User user)
    {
        var doctor = new Doctor
        {
            Status = DoctorStatus.Disable,
            Description = description,
            Education = education,
            User = user,
        };
        
        return doctor;
    }
    
    public void AssignWorkSchedule(WorkSchedule workSchedule)
    {
        WorkSchedule = workSchedule;
    }
    
    public Result AddSpecialization(Specialization specialization)
    {
        if (_specializations.Any(s => s.Id == specialization.Id))
            return Result.Failure(DoctorErrors.SpecializationAlreadyExists);
        
        _specializations.Add(specialization);
        
        return Result.Success();
    }
    
    public Result RemoveSpecialization(Guid specializationId)
    {
        var specialization = _specializations.FirstOrDefault(s => s.Id == specializationId);
        if (specialization is null)
            return Result.Failure(DoctorErrors.SpecializationNotFound);
        
        _specializations.Remove(specialization);
        
        return Result.Success();
    }
    
    public void ChangeStatus(DoctorStatus status)
    {
        Status = status;
    }
}
