using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Managers;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Managers.CreateManager;

internal sealed class CreateManagerCommandHandler(
    IUserRepository userRepository, 
    IManagerRepository managerRepository,
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager
    ) 
    : IRequestHandler<CreateMangerCommand, Result<ManagerResponse>>
{
    public async Task<Result<ManagerResponse>> Handle(CreateMangerCommand command, CancellationToken ct)
    {
        if (await userRepository.UserExistsByEmailAsync(command.Email, ct))
        {
            return Result.Failure<ManagerResponse>(UserErrors.AlreadyExist(command.Email));
        }
        
        passwordManager.CreatePasswordHash(command.Password, out var hash, out var salt);

        Result<Manager> result = Manager.Create(
            position: command.Position,
            user: User.Create(
                email: command.Email,
                passwordHash: hash,
                passwordSalt: salt,
                role: UserAuthRole.Manager,
                phone: command.Phone,
                firstName: command.FirstName,
                lastName: command.LastName
            ));

        if (result.IsFailure)
        {
            return Result.Failure<ManagerResponse>(result.Error);
        }
        
        await managerRepository.InsertAsync(result.Value, ct);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return ManagerResponse.FromManager(result.Value);
    }
}
