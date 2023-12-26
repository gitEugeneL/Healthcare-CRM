using Application.Operations.Users;
using Domain.Entities;

namespace Application.Operations.Doctor;

public sealed record DoctorResponse : UserResponse
{
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Education { get; set; }
    public List<Guid> SpecializationIds { get; set; } = [];
    
    public DoctorResponse ToDoctorResponse(UserDoctor doctor)
    {
        UserId = doctor.UserId;
        Email = doctor.User.Email;
        FirstName = doctor.User.FirstName;
        LastName = doctor.User.LastName;
        Phone = doctor.User.Phone;
        Status = doctor.Status.ToString();
        Description = doctor.Description;
        Education = doctor.Education;
        SpecializationIds = doctor.Specializations
            .Select(s => s.Id)
            .ToList();
        
        return this;
    }
}
