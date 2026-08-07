using Application.UseCases.Common.User;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Managers.CreateManager;

public sealed record CreateMangerCommand(
    string Email,
    string Password,
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Position
) : CreateUserCommand(Email, Password, FirstName, LastName, Phone), IRequest<Result<ManagerResponse>>;
