using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class UserDoctor : BaseAuditableEntity
{
    public Status Status { get; set; }
    public string? Description { get; set; }
    public string? Education { get; set; }
    
    /*** Relations ***/
    public required User User { get; set; }
    public Guid UserId { get; set; }

    public List<Specialization> Specializations { get; set; } = [];

    // todo appointment
    // todo config
    // todo medicalRecord
}
