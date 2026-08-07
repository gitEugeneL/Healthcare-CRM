using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Managers.CreateManager;

internal sealed class CreateManagerValidator : AbstractValidator<CreateMangerCommand>
{
    public CreateManagerValidator()
    {
        Include(new CreateUserValidator());

        RuleFor(m => m.Position)
            .MaximumLength(100)
            .WithMessage("The position must be less than 100 characters.");
    }
}