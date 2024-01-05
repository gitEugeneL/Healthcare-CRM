using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Operations.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    
    public LogoutCommandHandler(IUserRepository userRepository, IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _refreshTokenService = refreshTokenService;
    }
    
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // find user by token
        var user = await _userRepository.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken)
                   ?? throw new UnauthorizedException("Refresh token isn't valid");
        
        _refreshTokenService.ValidateAndRemoveRefreshToken(user, request.RefreshToken);
        
        await _userRepository.UpdateUserAsync(user, cancellationToken);
        return await Unit.Task;
    }
}
