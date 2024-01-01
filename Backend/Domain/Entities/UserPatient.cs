using Domain.Common;

namespace Domain.Entities;

public class UserPatient : BaseAuditableEntity
{
    public string? Pesel { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Insurance { get; set; }
    
    /*** Relations ***/    
    public required User User { get; init; }
    public Guid UserId { get; init; }

    public required Address Address { get; init; }
    public Guid AddressId { get; init; }

    public List<Appointment> Appointments { get; init; } = [];
    
    // medical records
}
