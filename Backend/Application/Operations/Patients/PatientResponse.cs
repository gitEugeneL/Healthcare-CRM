using Application.Operations.Users;
using Domain.Entities;

namespace Application.Operations.Patients;

public sealed record PatientResponse : UserResponse
{
    public string? Pesel { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Insurance { get; set; }
    public Guid? AddressId { get; set; }

    public PatientResponse ToPatientResponse(UserPatient patient)
    {
        UserId = patient.UserId;
        Email = patient.User.Email;
        FirstName = patient.User.FirstName;
        LastName = patient.User.LastName;
        Phone = patient.User.Phone;
        Pesel = patient.Pesel;
        DateOfBirth = patient.DateOfBirth.ToString();
        Insurance = patient.Insurance;
        AddressId = patient.AddressId;

        return this;
    }
}
