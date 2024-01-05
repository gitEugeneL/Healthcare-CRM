using Domain.Common;

namespace Domain.Entities;

public sealed class UserManager : BaseAuditableEntity
{
    public string? Position { get; set; }
    
    /*** Relations ***/
    public required User User { get; init; }
    public Guid UserId { get; init; }
}
