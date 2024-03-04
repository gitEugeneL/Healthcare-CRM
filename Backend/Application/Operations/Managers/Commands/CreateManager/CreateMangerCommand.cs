using Application.Operations.Users.Commands;
using FluentValidation;
using MediatR;

namespace Application.Operations.Managers.Commands.CreateManager;

public sealed record CreateMangerCommand : CreateUserCommand, IRequest<ManagerResponse>;

public sealed class CreateMangerValidator : AbstractValidator<CreateMangerCommand>
{
    public CreateMangerValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
            .WithMessage("The password must contain at least one letter, one special character, and one digit.");
    }
}
