using Domain.Common;

namespace Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
    
    /**** Relations ****/
    public required User User { get; set; }
    public Guid UserId { get; set; }
}
