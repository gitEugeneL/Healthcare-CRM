using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.Refresh;

public sealed record RefreshCommand(
    string Email,
    string RefreshTokenValue
) : IRequest<Result<LoginOrRefreshResponse>>;