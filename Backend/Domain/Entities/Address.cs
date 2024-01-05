using Domain.Common;

namespace Domain.Entities;

public class Address : BaseAuditableEntity
{
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Hose { get; set; }
    public string? Apartment { get; set; }
    
    /*** Relations ***/
    public UserPatient? UserPatient { get; init; }
}
