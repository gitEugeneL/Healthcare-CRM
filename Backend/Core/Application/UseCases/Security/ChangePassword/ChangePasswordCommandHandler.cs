using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Security.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    ILockoutService lockoutService,
    IConfirmationService confirmationService,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork
) : IRequestHandler<ChangePasswordCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ChangePasswordCommand command, CancellationToken ct)
    {
        var currentLockoutState = await lockoutService.CheckConfirmLockoutState(command.Email, ct);
        if (currentLockoutState.IsLocked)
            return Result.Failure<Unit>(UserErrors.NotFoundOrCodeIsInvalid);

        var user = await userRepository.FindUserByEmailAsync(command.Email, ct);
        
        if (user is null || 
            !user.IsEmailConfirmed || 
            !await confirmationService.ValidateCodeAsync(command.Email, command.ConfirmationCode))
        {
            await lockoutService.ProcessForConfirmAsync(command.Email, false, ct);
            return Result.Failure<Unit>(UserErrors.NotFoundOrCodeIsInvalid);
        }
        
        passwordService.CreatePasswordHash(command.NewPassword, out var passwordHash, out var passwordSalt);
        
        user.ChangePassword(passwordHash, passwordSalt);
        await unitOfWork.SaveChangesAsync(ct);
        
        return Unit.Value;
        
    }
}