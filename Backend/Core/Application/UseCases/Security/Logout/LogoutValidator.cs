using FluentValidation;

namespace Application.UseCases.Security.Logout;

internal sealed class LogoutValidator : AbstractValidator<LogoutCommand>
{
    public LogoutValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
        
        RuleFor(c => c.RefreshTokenValue)
            .NotEmpty()
            .WithMessage("The refresh token is required.");      
    }
}