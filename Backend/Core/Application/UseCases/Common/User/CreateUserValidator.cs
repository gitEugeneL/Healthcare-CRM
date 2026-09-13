using FluentValidation;

namespace Application.UseCases.Common.User;

internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
        
        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Length(8, 200)
            .WithMessage("Password must be between 8 and 150 characters")
            .Must(p => p.Any(char.IsLetter))
            .WithMessage("Password must contain letters")
            .Must(p => p.Any(char.IsUpper))
            .WithMessage("Password must contain upper case")
            .Must(p => p.Any(char.IsDigit))
            .WithMessage("Password must contain digits")
            .Must(p => p.Any(c => !char.IsLetterOrDigit(c)))
            .WithMessage("Password must contain special characters");     
        
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