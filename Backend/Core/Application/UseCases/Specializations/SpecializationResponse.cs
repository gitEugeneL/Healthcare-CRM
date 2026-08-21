using Domain.Specializations;

namespace Application.UseCases.Specializations;

public sealed record SpecializationResponse(
    Guid SpecializationId,
    string Name,
    string? Description,
    IReadOnlyCollection<Doctor> Doctors)
{
    public static SpecializationResponse FromSpecialization(Specialization specialization)
    {
        return new SpecializationResponse(
            SpecializationId: specialization.Id,
            Name: specialization.Name,
            Description: specialization.Description, 
            Doctors: specialization.Doctors
                .Select(d => new Doctor(
                    d.Id, 
                    d.Status.ToString(),
                    d.User.FirstName,
                    d.User.LastName))
                .ToList());
    }
}

public sealed record Doctor(Guid DoctorId, string Status, string? FirstName, string? LastName);
