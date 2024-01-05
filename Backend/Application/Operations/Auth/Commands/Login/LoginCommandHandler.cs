using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private const int RefreshTokensCount = 5; // each user can only have 5 refresh tokens
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenManager _tokenManager;

    public LoginCommandHandler(
        IUserRepository userRepository, IPasswordManager passwordManager, ITokenManager tokenManager)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _tokenManager = tokenManager;
    }
    
    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email, cancellationToken);
        
        if (user is null || !_passwordManager.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new AccessDeniedException(nameof(User), request.Email);
        
        if (user.RefreshTokens.Count >= RefreshTokensCount)
        {
            var oldestRefreshToken = user.RefreshTokens
                .OrderBy(rt => rt.Expires)
                .First();

            user.RefreshTokens.Remove(oldestRefreshToken);
        }
        
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
