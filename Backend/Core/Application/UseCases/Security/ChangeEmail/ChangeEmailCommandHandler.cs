using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.ChangeEmail;

internal sealed class ChangeEmailCommandHandler(
    IUserRepository userRepository,
    ILockoutService lockoutService,
    IConfirmationService confirmationService,
    IUnitOfWork unitOfWork
) : IRequestHandler<ChangeEmailCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ChangeEmailCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckConfirmLockoutState(command.CurrentUserEmail, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<Unit>(UserErrors.NotFoundOrCodeIsInvalid);
        
        var user = await userRepository.FindUserByEmailAsync(command.CurrentUserEmail, ct);
        
        if (user is null ||
            !user.IsEmailConfirmed || 
            !await confirmationService.ValidateCodeAsync(command.CurrentUserEmail, command.ConfirmationCode))
        {
            await lockoutService.ProcessForConfirmAsync(command.CurrentUserEmail, false, ct);
            return Result.Failure<Unit>(UserErrors.NotFoundOrCodeIsInvalid);
        }

        if (await userRepository.UserExistsByEmailAsync(command.NewEmail, ct))
        {
            return Result.Failure<Unit>(UserErrors.AlreadyExist(command.NewEmail));
        }
        
        user.ChangeEmail(command.NewEmail);
        await unitOfWork.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}