using FluentValidation;
using MediatR;

namespace Application.Operations.Auth.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password
) : IRequest<Guid>;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}