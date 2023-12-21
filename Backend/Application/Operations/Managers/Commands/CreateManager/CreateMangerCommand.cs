using Application.Operations.Common.Users.Commands;
using MediatR;

namespace Application.Operations.Managers.Commands.CreateManager;

public sealed record CreateMangerCommand : CreateUserCommand, IRequest<ManagerResponse>;
