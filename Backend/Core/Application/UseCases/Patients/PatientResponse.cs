using Application.UseCases.Common.User;
using Domain.Patients;

namespace Application.UseCases.Patients;

public sealed record PatientResponse(
    Guid PatientId,
    string Status,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    DateOnly DateOfBirth,
    string? Insurance,
    PatientAddress Address
) : UserResponse(Email, FirstName, LastName, Phone)
{
    public static PatientResponse FromPatient(Patient patient)
    {
        return new PatientResponse(
            PatientId: patient.Id,
            Email: patient.User.Email,
            FirstName: patient.User.FirstName,
            LastName: patient.User.LastName,
            Phone: patient.User.Phone,
            Status: patient.Status.ToString(),
            DateOfBirth: patient.DateOfBirth,
            Insurance: patient.Insurance,
            Address: new PatientAddress(
                Province: patient.Address.Province,
                PostalCode: patient.Address.PostalCode,
                City: patient.Address.City,
                Street: patient.Address.Street,
                Hose: patient.Address.Hose,
                Apartment: patient.Address.Apartment));
    }
}
