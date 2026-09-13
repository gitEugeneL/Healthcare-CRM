using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.ChangePassword;

public sealed record ChangePasswordCommand(
    string Email,
    string ConfirmationCode,
    string NewPassword,
    string NewPasswordConfirmation
) : IRequest<Result<Unit>>;