using Domain.Addresses;
using Domain.Common;
using Domain.Users;

namespace Domain.Patients;

public sealed class Patient : BaseAuditableEntity
{
    public DateOnly DateOfBirth { get; private set; }
    public string Pesel { get; private set; } = null!;
    public string? Insurance { get; private set; }
    
    public PatientStatus Status { get; private set; }
    
    /*** Relations ***/    
    public User User { get; private init; } = null!;
    public Guid UserId { get; private init; }

    public Address Address { get; private init; } = null!;
    public Guid AddressId { get; private init; }
    
    // public List<Appointment> Appointments { get; init; } = [];

    // public List<MedicalRecord> MedicalRecords { get; init; } = [];

    public static Patient Create(DateOnly dateOfBirth, string pesel, string? insurance, User user, Address address)
    {
        var patient = new Patient
        {   
            Status = PatientStatus.Active,
            DateOfBirth = dateOfBirth,
            Pesel = pesel.Trim().ToUpperInvariant(),
            Insurance = insurance?.Trim(),
            User = user,
            Address = address
        };
        return patient;
    }

    public void ChangeDateOfBirth(DateOnly dateOfBirth)
    {
        if (DateOfBirth == dateOfBirth)
            return;
        
        DateOfBirth = dateOfBirth;
    }

    public void ChangePesel(string pesel)
    {
        var normalize = pesel.Trim().ToUpperInvariant();
        
        if (Pesel == normalize)
            return;
        
        Pesel = normalize;
    }
    
    public void ChangeInsurance(string? insurance)
    {
        var normalize = insurance?.Trim();

        if (Insurance == normalize)
            return;
        
        Insurance = normalize;
    }
    
    public void Deactivate()
    {
        Status = PatientStatus.Deleted;
    }
}
