using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.ChangeEmail;

public sealed record ChangeEmailCommand(
    string NewEmail,
    string CurrentUserEmail,
    string ConfirmationCode 
) : IRequest<Result<Unit>>;