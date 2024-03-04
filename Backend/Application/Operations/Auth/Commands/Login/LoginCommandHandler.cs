using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    ITokenManager tokenManager)
    : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private const int RefreshTokensCount = 5; // each user can only have 5 refresh tokens

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUserByEmailAsync(request.Email, cancellationToken);
        
        if (user is null || !passwordManager.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new AccessDeniedException(nameof(User), request.Email);
        
        if (user.RefreshTokens.Count >= RefreshTokensCount)
        {
            var oldestRefreshToken = user.RefreshTokens
                .OrderBy(rt => rt.Expires)
                .First();

            user.RefreshTokens.Remove(oldestRefreshToken);
        }
        
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
