using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.Logout;

public sealed record LogoutCommand(
    string Email,
    string RefreshTokenValue
) : IRequest<Result<Unit>>;