using FluentValidation;

namespace Application.UseCases.Security.Refresh;

internal sealed class RefreshValidator : AbstractValidator<RefreshCommand>
{
    public RefreshValidator()
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