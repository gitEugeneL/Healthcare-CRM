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
            Name = name,
            Description = description
        };
        
        return specialization;
    }
}
