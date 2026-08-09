using Application.UseCases.Common.User;
using Domain.Doctors;

namespace Application.UseCases.Doctors;

public sealed record DoctorResponse(
    Guid DoctorId,
    IReadOnlyCollection<DoctorSpecializations> Specializations,
    string Status,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Description,
    string? Education
) : UserResponse(Email, FirstName, LastName, Phone)
{
    public static DoctorResponse FromDoctor(Doctor doctor)
    {
        return new DoctorResponse(
            DoctorId: doctor.Id,
            Email: doctor.User.Email,
            FirstName: doctor.User.FirstName,
            LastName: doctor.User.LastName,
            Phone: doctor.User.Phone,
            Status: doctor.Status.ToString(),
            Description: doctor.Description,
            Education: doctor.Education,
            Specializations: doctor.Specializations
                .Select(s => new DoctorSpecializations(s.Id, s.Name))
                .ToList());
    }
}

public sealed record DoctorSpecializations(
        Guid SpecializationId, 
        string Name); 