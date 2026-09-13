using Application.Abstractions.Security;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.GenerateCode;

internal sealed class GenerateCodeCommandHandler(
    IUserRepository userRepository,
    IConfirmationService confirmationService,
    ILockoutService lockoutService
) : IRequestHandler<GenerateCodeCommand, Result<GenerateCodeResponse>>
{
    public async Task<Result<GenerateCodeResponse>> Handle(GenerateCodeCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckGenerateCodeLockoutState(command.Email, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<GenerateCodeResponse>(UserErrors.NotFoundOrInvalidOrExpiredToken);

        var user = await userRepository.FindUserByEmailWithRefreshTokensAsync(command.Email, ct);
        if (user is null)
        {
            await lockoutService.ProcessForGenerateCodeAsync(command.Email, ct);
            return Result.Failure<GenerateCodeResponse>(UserErrors.NotFoundOrInvalidOrExpiredToken);
        }

        await lockoutService.ProcessForGenerateCodeAsync(user.Email, ct);
        var codeExpiration = await confirmationService.GenerateCodeAsync(user.Email);
        
        return new GenerateCodeResponse(user.Email, codeExpiration);
    }
}