using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class UserDoctor : BaseAuditableEntity
{
    public Status Status { get; set; }
    public string? Description { get; set; }
    public string? Education { get; set; }
    
    /*** Relations ***/
    public required User User { get; init; }
    public Guid UserId { get; init; }

    public required AppointmentSettings AppointmentSettings { get; init; }
    public Guid AppointmentSettingsId { get; init; }
    
    public List<Specialization> Specializations { get; init; } = [];

    // todo appointment
    // todo medicalRecord
}
