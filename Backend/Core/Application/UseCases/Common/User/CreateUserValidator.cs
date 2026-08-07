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
        
        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("The password is required.")
            .MinimumLength(8)
            .WithMessage("The password must be at least 8 characters long.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d\s]).{8,64}$")
            .WithMessage("The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");       
        
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