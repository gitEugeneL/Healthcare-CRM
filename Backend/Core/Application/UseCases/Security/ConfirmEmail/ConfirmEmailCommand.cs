using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.ConfirmEmail;

public sealed record ConfirmEmailCommand(
    string Email,
    string ConfirmationCode
) : IRequest<Result<Unit>>;