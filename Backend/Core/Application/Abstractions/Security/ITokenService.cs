using Domain.RefreshTokens;
using Domain.Users;

namespace Application.Abstractions.Security;

public interface ITokenService
{
    (string token, DateTime expires) GenerateAccessToken(User user);

    RefreshToken GenerateRefreshToken(User user);

    void AddRefreshTokenToUser(User user, RefreshToken refreshToken);

    void RemoveRefreshTokenFromUser(User user, RefreshToken refreshToken);
}