using Application.Operations.Users.Commands;
using MediatR;

namespace Application.Operations.Managers.Commands.UpdateManager;

public sealed record UpdateManagerCommand : UpdateUserCommand, IRequest<ManagerResponse>
{
    public string? Position { get; set; }
}
