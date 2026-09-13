using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Security.ChangeEmail;

internal sealed class ChangeEmailValidator : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailValidator(IConfiguration configuration)
    {
        var codeLength = int.Parse(configuration["Authentication:ConfirmationCode:Length"] ??
                                   throw new ApplicationException("Code.Length not found in configuration"));

        RuleFor(c => c.CurrentUserEmail)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
        
        RuleFor(c => c.NewEmail)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
        
        RuleFor(c => c.ConfirmationCode)
            .NotEmpty()
            .WithMessage("Code is required")
            .Length(codeLength)
            .WithMessage("Code is invalid");
    }
}