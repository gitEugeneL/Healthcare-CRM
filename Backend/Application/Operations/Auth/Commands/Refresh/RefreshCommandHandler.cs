using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthenticationResponse>
{
    private readonly ITokenManager _tokenManager;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshCommandHandler(
        ITokenManager tokenManager, IUserRepository userRepository, IRefreshTokenService refreshTokenService)
    {
        _tokenManager = tokenManager;
        _userRepository = userRepository;
        _refreshTokenService = refreshTokenService;
    }
    
    public async Task<AuthenticationResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        // find user by token
        var user = await _userRepository.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken)
                   ?? throw new UnauthorizedException("Refresh token isn't valid");
        
        _refreshTokenService.ValidateAndRemoveRefreshToken(user, request.RefreshToken);
        
        var accessToken = _tokenManager.GenerateAccessToken(user);
        var refreshToken = _tokenManager.GenerateRefreshToken(user);
        
        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUserAsync(user, cancellationToken);
        
        return new AuthenticationResponse(
            new JwtToken(accessToken),
            new CookieToken(refreshToken.Token, refreshToken.Expires)
        );
    }
}
