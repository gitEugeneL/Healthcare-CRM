using Domain.Abstractions.Errors;

namespace Domain.RefreshTokens;

public static class RefreshTokenErrors
{
    public static readonly Error ExpiresPrecedesUtcNow = Error.Problem(
        "RefreshToken.ExpiresPrecedesUtcNow",
        "The expires date time precedes the UTC now");
}