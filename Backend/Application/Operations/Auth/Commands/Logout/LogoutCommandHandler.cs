using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Operations.Auth.Commands.Logout;

public class LogoutCommandHandler(
    IUserRepository userRepository, 
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken)
                   ?? throw new UnauthorizedException("Refresh token isn't valid");
        
        refreshTokenService.ValidateAndRemoveRefreshToken(user, request.RefreshToken);
        
        await userRepository.UpdateUserAsync(user, cancellationToken);
        return await Unit.Task;
    }
}
