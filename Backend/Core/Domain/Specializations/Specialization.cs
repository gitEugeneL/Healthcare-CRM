using Domain.Abstractions.Errors;
using Domain.Common;
using Domain.Doctors;

namespace Domain.Specializations;

public sealed class Specialization : BaseEntity
{
    private Specialization() { }
    
    public string Name { get; private init; } = null!;
    public string? Description { get; private set; }
    
    /*** Relations ***/
    private readonly List<Doctor> _doctors = [];
    public IReadOnlyList<Doctor> Doctors => _doctors.AsReadOnly();
    
    public static Specialization Create(string name, string? description)
    {
        var specialization = new Specialization
        {
            Name = name.Trim().ToUpperInvariant(),
            Description = description?.Trim()
        };
        
        return specialization;
    }
    
    public void UpdateDescription(string description)
    {
        var normalized = description.Trim();
        
        if (Description == normalized)
            return;
        
        Description = normalized;
    }
    
    public Result IncludeDoctor(Doctor doctor)
    {
        if (_doctors.Any(d => d.Id == doctor.Id))
        {
            return Result.Failure(SpecializationErrors.DoctorAlreadyIncluded(doctor.Id));
        }
        _doctors.Add(doctor);
        return Result.Success();
    }

    public Result ExcludeDoctor(Doctor doctor)
    {
        var result = _doctors.FirstOrDefault(d => d.Id == doctor.Id);
        if (result is null)
        {
            return Result.Failure(SpecializationErrors.DoctorNotFound(doctor.Id));
        }
        _doctors.Remove(result);
        return Result.Success();
    }
}
