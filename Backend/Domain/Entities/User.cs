using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required Role Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    
    /*** Relations ***/
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public UserManager? UserManager { get; set; }
}
