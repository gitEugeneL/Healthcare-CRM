using FluentValidation;

namespace Application.UseCases.Specializations.UpdateSpecialization;

internal sealed class UpdateSpecializationValidator : AbstractValidator<UpdateSpecializationCommand>
{
    public UpdateSpecializationValidator()
    {
        RuleFor(s => s.SpecializationId)
            .NotEmpty()
            .WithMessage("The specialization id is required.");
        
        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("The description is required.")
            .MaximumLength(250)
            .WithMessage("The description must be less than 250 characters."); 
    }
}