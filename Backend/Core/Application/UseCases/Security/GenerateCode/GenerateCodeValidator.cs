using FluentValidation;

namespace Application.UseCases.Security.GenerateCode;

internal sealed class GenerateCodeValidator : AbstractValidator<GenerateCodeCommand>
{
    public GenerateCodeValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
    }
}