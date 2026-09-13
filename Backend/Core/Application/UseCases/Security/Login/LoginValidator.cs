using FluentValidation;

namespace Application.UseCases.Security.Login;

internal sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
        
        RuleFor(l => l.Password)
            .NotEmpty()
            .WithMessage("The password is required.");       
    }
}