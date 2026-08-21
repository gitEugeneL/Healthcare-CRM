using FluentValidation;

namespace Application.UseCases.Specializations.CreateSpecialization;

internal sealed class CreateSpecializationValidator : AbstractValidator<CreateSpecializationCommand>
{
    public CreateSpecializationValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(150)
            .WithMessage("The name must be less than 150 characters.");
        
        RuleFor(s => s.Description)
            .MaximumLength(250)
            .WithMessage("The description must be less than 250 characters.");      
    }
}