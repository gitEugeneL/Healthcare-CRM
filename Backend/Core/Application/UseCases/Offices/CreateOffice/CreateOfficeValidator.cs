using FluentValidation;

namespace Application.UseCases.Offices.CreateOffice;

internal sealed class CreateOfficeValidator : AbstractValidator<CreateOfficeCommand>
{
    public CreateOfficeValidator()
    {
        RuleFor(o => o.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(50)
            .WithMessage("The name must be less than 50 characters.");

        RuleFor(o => o.Number)
            .NotEmpty()
            .WithMessage("The number is required.")
            .InclusiveBetween(1, 9999)
            .WithMessage("The number must be between 1 and 9999.");       
    }
}