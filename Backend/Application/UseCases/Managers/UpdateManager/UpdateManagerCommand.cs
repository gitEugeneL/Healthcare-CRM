using Application.UseCases.Common.User;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Managers.UpdateManager;

public sealed record UpdateManagerCommand(
    Guid UserId,
    string? Phone,
    string? Position,
    string? FirstName,
    string? LastName
) : UpdateUserCommand(UserId, FirstName, LastName, Phone), IRequest<Result<ManagerResponse>>;
