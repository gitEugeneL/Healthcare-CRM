using Domain.RefreshTokens;
using Domain.Users;

namespace Application.Common.Interfaces;

public interface ITokenManager
{
    string GenerateAccessToken(User user);
    
    RefreshToken GenerateRefreshToken(User user);
}
