using Domain.Abstractions.Errors;
using Domain.Common;
using Domain.Users;

namespace Domain.RefreshTokens;

public sealed class RefreshToken : BaseEntity
{
    private RefreshToken() { }

    public string Token { get; private init; } = null!;
    public DateTime Expires { get; private init; }
    
    /**** Relations ****/
    public User User { get; private init; } = null!;
    public Guid UserId { get; private init; }


    public static Result<RefreshToken> Create(string token, DateTime expires, Guid userId)
    {
        if (expires < DateTime.UtcNow)
            return Result.Failure<RefreshToken>(RefreshTokenErrors.ExpiresPrecedesUtcNow);

        var refreshToken = new RefreshToken
        {
            Token = token,
            Expires = expires,
            UserId = userId
        };

        return refreshToken;
    }
}
