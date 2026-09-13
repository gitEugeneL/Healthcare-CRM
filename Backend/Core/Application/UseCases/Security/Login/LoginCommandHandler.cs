using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork,
    ILockoutService lockoutService,
    ITokenService tokenService
) : IRequestHandler<LoginCommand, Result<LoginOrRefreshResponse>>
{
    
    public async Task<Result<LoginOrRefreshResponse>> Handle(LoginCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckLoginLockoutAsync(command.Email, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<LoginOrRefreshResponse>(UserErrors.NotFoundOfInvalidPassword(command.Email));
        
        var user = await userRepository.FindUserByEmailWithRefreshTokensAsync(command.Email, ct);
        if (user is null)
        {
            await lockoutService.ProcessForLoginAsync(command.Email, false, ct);
            return Result.Failure<LoginOrRefreshResponse>(UserErrors.NotFoundOfInvalidPassword(command.Email));
        }

        var isPasswordValid = passwordService
            .VerifyPasswordHash(command.Password, user.PasswordHash, user.PasswordSalt);

         await lockoutService.ProcessForLoginAsync(user.Email, isPasswordValid, ct);

         if (!isPasswordValid)
            return Result.Failure<LoginOrRefreshResponse>(UserErrors.NotFoundOfInvalidPassword(command.Email));
        
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