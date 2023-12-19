using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITokenManager
{
    string GenerateAccessToken(User user);
    
    RefreshToken GenerateRefreshToken(User user);
}
