using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.Logout;

internal sealed class LogoutCommandHandler(
    IUserRepository userRepository,
    ILockoutService lockoutService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork
) : IRequestHandler<LogoutCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(LogoutCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckLoginLockoutAsync(command.Email, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<Unit>(UserErrors.NotFoundOrInvalidOrExpiredToken);
        
        var user = await userRepository.FindUserByEmailWithRefreshTokensAsync(command.Email, ct);
   
        if (user is null)
        {
            await lockoutService.ProcessForLoginAsync(command.Email, false, ct);
            return Result.Failure<Unit>(UserErrors.NotFoundOrInvalidOrExpiredToken);
        }
        
        var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == command.RefreshTokenValue);
        if (refreshToken is null)
        {
            await lockoutService.ProcessForLoginAsync(command.Email, false, ct);
            return Result.Failure<Unit>(UserErrors.NotFoundOrInvalidOrExpiredToken);
        } 
        
        tokenService.RemoveRefreshTokenFromUser(user, refreshToken);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}