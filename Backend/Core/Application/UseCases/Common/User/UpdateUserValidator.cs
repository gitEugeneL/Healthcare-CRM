using FluentValidation;

namespace Application.UseCases.Common.User;

internal sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.FirstName)
            .MaximumLength(50)
            .WithMessage("The first name must be less than 50 characters.");
        
        RuleFor(u => u.LastName)
            .MaximumLength(50)
            .WithMessage("The last name must be less than 50 characters.");       
        
        RuleFor(m => m.Phone)
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone must be a valid international phone number.");
    }
}