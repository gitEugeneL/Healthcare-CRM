using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    public required string Email { get; init; }
    public required byte[] PasswordHash { get; init; }
    public required byte[] PasswordSalt { get; init; }
    public required Role Role { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    
    /*** Relations ***/
    public List<RefreshToken> RefreshTokens { get; init; } = [];
    public UserManager? UserManager { get; init; }
    public UserDoctor? UserDoctor { get; init; }
    public UserPatient? UserPatient { get; init; }
}
