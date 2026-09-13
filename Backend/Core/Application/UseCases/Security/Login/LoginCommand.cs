using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<LoginOrRefreshResponse>>;