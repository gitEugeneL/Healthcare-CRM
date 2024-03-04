using FluentValidation;
using MediatR;

namespace Application.Operations.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthenticationResponse>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(l => l.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
