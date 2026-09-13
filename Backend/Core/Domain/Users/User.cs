using Domain.Common;
using Domain.RefreshTokens;

namespace Domain.Users;

public sealed class User : BaseAuditableEntity
{
    private User() { }

    public string Email { get; private set; } = null!;
    public bool IsTemporaryPassword { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public UserAuthRole Role { get; private init; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Phone { get; private set; }
    
    /*** Relations ***/
    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    
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
            IsTemporaryPassword = true,
            IsEmailConfirmed = false,
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = role,
            FirstName = firstName?.Trim().Trim(),
            LastName = lastName?.Trim().Trim(),
            Phone = phone?.Trim()
        };

        return user;
    }
    
    public void UpdateRefreshTokens(RefreshToken refreshToken, int maxActiveTokens)
    {
        _refreshTokens.RemoveAll(t => t.Expires <= DateTime.UtcNow);

        while (_refreshTokens.Count >= maxActiveTokens)
        {
            var oldestToken = _refreshTokens.OrderBy(t => t.Expires).First();
            _refreshTokens.Remove(oldestToken);
        }

        _refreshTokens.Add(refreshToken);
    }

    public void RemoveRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Remove(refreshToken);
    }

    public void ConfirmEmail()
    {
        if (IsEmailConfirmed)
            return;

        IsEmailConfirmed = true;
    }

    public void UnconfirmEmail()
    {
        if (!IsEmailConfirmed)
            return;
        
        IsEmailConfirmed = false;
    }

    public void ChangeEmail(string newEmail)
    {
        Email = newEmail.Trim().ToLowerInvariant();
        IsEmailConfirmed = false;
    }
    
    public void ChangePassword(byte[] newHash, byte[] newSalt)
    {
        PasswordHash = newHash;
        PasswordSalt = newSalt;
        IsTemporaryPassword = false;
    }
    
    public void ChangeFirstName(string firstName)
    {
        var normalized = firstName.Trim().ToUpperInvariant();
        
        if (FirstName == normalized)
            return;
        
        FirstName = normalized;
    }
    
    public void ChangeLastName(string lastName)
    {
        var normalized = lastName.Trim().ToUpperInvariant();
        
        if (LastName == normalized)
            return;

        LastName = normalized;
    }
    
    public void ChangePhone(string phone)
    {
        var normalized = phone.Trim();
        
        if (Phone == normalized)
            return;

        Phone = normalized;
    }
}
