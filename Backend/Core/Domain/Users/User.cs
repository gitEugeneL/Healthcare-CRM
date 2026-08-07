using Domain.Common;
using Domain.RefreshTokens;

namespace Domain.Users;

public sealed class User : BaseAuditableEntity
{
    private User() { }

    public string Email { get; private init; } = null!;
    public byte[] PasswordHash { get; private init; } = null!;
    public byte[] PasswordSalt { get; private init; } = null!;
    public UserAuthRole Role { get; private init; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Phone { get; private set; }
    
    /*** Relations ***/
    public List<RefreshToken> RefreshTokens { get; private init; } = [];

    public static User Create(
        string email,
        byte[] passwordHash,
        byte[] passwordSalt,
        UserAuthRole role,
        string? firstName,
        string? lastName,
        string? phone)
    {
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = role,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone
        };

        return user;
    }

    public void ChangeFirstName(string firstName)
    {
        if (FirstName == firstName)
        {
            return;
        }
        FirstName = firstName;
    }
    
    public void ChangeLastName(string lastName)
    {
        if (LastName == lastName)
        {
            return;
        }
        LastName = lastName;
    }
    
    public void ChangePhone(string phone)
    {
        if (Phone == phone)
        {
            return;
        }
        Phone = phone;
    }
}
