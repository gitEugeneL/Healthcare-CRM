using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler(
    IUserRepository userRepository,
    ILockoutService lockoutService,
    IConfirmationService confirmationService,
    IUnitOfWork unitOfWork
) : IRequestHandler<ConfirmEmailCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ConfirmEmailCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckConfirmLockoutState(command.Email, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<Unit>(UserErrors.NotFoundOrCodeIsInvalid);
        
        var user = await userRepository.FindUserByEmailWithRefreshTokensAsync(command.Email, ct);
        
        if (user is null || !await confirmationService.ValidateCodeAsync(command.Email, command.ConfirmationCode))
        {
            await lockoutService.ProcessForConfirmAsync(command.Email, false, ct);
            return Result.Failure<Unit>(UserErrors.NotFoundOrCodeIsInvalid);
        }
        
        user.ConfirmEmail();
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}