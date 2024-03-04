using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Auth.Commands.Refresh;

public class RefreshCommandHandler(
    ITokenManager tokenManager,
    IUserRepository userRepository,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<RefreshCommand, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken)
                   ?? throw new UnauthorizedException("Refresh token isn't valid");
        
        refreshTokenService.ValidateAndRemoveRefreshToken(user, request.RefreshToken);
        
        var accessToken = tokenManager.GenerateAccessToken(user);
        var refreshToken = tokenManager.GenerateRefreshToken(user);
        
        user.RefreshTokens.Add(refreshToken);
        await userRepository.UpdateUserAsync(user, cancellationToken);
        
        return new AuthenticationResponse(
            new JwtToken(accessToken),
            new CookieToken(refreshToken.Token, refreshToken.Expires)
        );
    }
}
