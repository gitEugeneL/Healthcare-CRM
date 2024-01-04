using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Managers.Commands.UpdateManager;

public class UpdateManagerCommandHandler(IManagerRepository managerRepository)
    : IRequestHandler<UpdateManagerCommand, ManagerResponse>
{
    public async Task<ManagerResponse> Handle(UpdateManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = await managerRepository.FindManagerByUserIdAsync(request.GetCurrentUserId(), cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());
        
        manager.User.FirstName = request.FirstName ?? manager.User.FirstName;
        manager.User.LastName = request.LastName ?? manager.User.LastName;
        manager.User.Phone = request.Phone ?? manager.User.Phone;
        manager.Position = request.Position ?? manager.Position;

        var updatedManager = await managerRepository.UpdateManagerAsync(manager, cancellationToken);
        return new ManagerResponse()
            .ToManagerResponse(updatedManager);
    }
}