using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Managers;
using MediatR;

namespace Application.UseCases.Managers.UpdateManager;

internal sealed class UpdateManagerCommandHandler(
    IManagerRepository managerRepository,
    IUnitOfWork unitOfWork
): IRequestHandler<UpdateManagerCommand, Result<ManagerResponse>>
{
    public async Task<Result<ManagerResponse>> Handle(UpdateManagerCommand command, CancellationToken ct)
    {
        var manager = await managerRepository.FindManagerByIdWithTrackingAsync(command.ManagerId, ct);
        if (manager is null)
            return Result.Failure<ManagerResponse>(ManagerErrors.NotFound(command.ManagerId));

        if (command.Position is not null)
            manager.UpdatePosition(command.Position);
        
        if (command.FirstName is not null)
            manager.User.ChangeFirstName(command.FirstName);

        if (command.LastName is not null)
            manager.User.ChangeLastName(command.LastName);

        if (command.Phone is not null)
            manager.User.ChangePhone(command.Phone);

        await unitOfWork.SaveChangesAsync(ct);
        
        return ManagerResponse.FromManager(manager);
    }
}