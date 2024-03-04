using Application.Operations.Users.Commands;
using FluentValidation;
using MediatR;

namespace Application.Operations.Managers.Commands.UpdateManager;

public sealed record UpdateManagerCommand(
    string? Position
) : UpdateUserCommand, IRequest<ManagerResponse>;

public sealed class UpdateManagerValidator : AbstractValidator<UpdateManagerCommand>
{
    public UpdateManagerValidator()
    {
        RuleFor(d => d.FirstName)
            .MaximumLength(50);

        RuleFor(d => d.LastName)
            .MaximumLength(100);

        RuleFor(d => d.Phone)
            .MaximumLength(12)
            .Matches("^[+]?\\d+$")
            .WithMessage("Phone number should start with + (optional) and contain only digits.");
    }
}

