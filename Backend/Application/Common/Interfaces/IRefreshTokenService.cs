using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRefreshTokenService
{
    void ValidateAndRemoveRefreshToken(User user, string oldRefreshToken);
}
