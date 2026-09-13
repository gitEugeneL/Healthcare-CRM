using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.Refresh;

internal sealed class RefreshCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService,
    ILockoutService lockoutService,
    IUnitOfWork unitOfWork
) : IRequestHandler<RefreshCommand, Result<LoginOrRefreshResponse>>
{
    public async Task<Result<LoginOrRefreshResponse>> Handle(RefreshCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckLoginLockoutAsync(command.Email, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<LoginOrRefreshResponse>(UserErrors.NotFoundOrInvalidOrExpiredToken);

        var user = await userRepository.FindUserByEmailWithRefreshTokensAsync(command.Email, ct);
   
        if (user is null)
        {
            await lockoutService.ProcessForLoginAsync(command.Email, false, ct);
            return Result.Failure<LoginOrRefreshResponse>(UserErrors.NotFoundOrInvalidOrExpiredToken);
        }

        var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == command.RefreshTokenValue);
        if (refreshToken is null || refreshToken.Expires <= DateTime.UtcNow)
        {
            await lockoutService.ProcessForLoginAsync(command.Email, false, ct);
            return Result.Failure<LoginOrRefreshResponse>(UserErrors.NotFoundOrInvalidOrExpiredToken);
        } 
        
        var generateAssesTokenResult = tokenService.GenerateAccessToken(user);
        var generateRefreshTokenResult = tokenService.GenerateRefreshToken(user);

        tokenService.AddRefreshTokenToUser(user, generateRefreshTokenResult);
        
        await unitOfWork.SaveChangesAsync(ct);
     
        return LoginOrRefreshResponse.FromTokens(
            accessToken: generateAssesTokenResult.token,
            accessTokenExpires: generateAssesTokenResult.expires,
            refreshToken: generateRefreshTokenResult);
    }
}