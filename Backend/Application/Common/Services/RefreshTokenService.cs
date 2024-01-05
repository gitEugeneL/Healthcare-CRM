using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Common.Services;

internal class RefreshTokenService : IRefreshTokenService
{
    public void ValidateAndRemoveRefreshToken(User user, string oldRefreshToken)
    {
        // find this token in the user
        var userRefreshToken = user.RefreshTokens
            .First(rt => rt.Token == oldRefreshToken);
        
        // check refresh token expiration time
        if (userRefreshToken.Expires < DateTime.UtcNow)
            throw new UnauthorizedException("Refresh token is outdated"); 
        
        // remove old refresh token
        user.RefreshTokens.Remove(userRefreshToken);
    }
}
