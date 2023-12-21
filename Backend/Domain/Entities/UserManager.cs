using Domain.Common;

namespace Domain.Entities;

public sealed class UserManager : BaseAuditableEntity
{
    public string? Position { get; set; }
    
    /*** Relations ***/
    public required User User { get; set; }
    public Guid UserId { get; set; }
}
