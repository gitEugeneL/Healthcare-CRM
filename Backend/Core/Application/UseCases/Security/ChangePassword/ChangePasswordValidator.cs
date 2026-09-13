using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Security.ChangePassword;

internal sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator(IConfiguration configuration)
    {
        var codeLength = int.Parse(configuration["Authentication:ConfirmationCode:Length"] ??
                                   throw new ApplicationException("Code.Length not found in configuration"));
        
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email is not valid.");
        
        RuleFor(c => c.ConfirmationCode)
            .NotEmpty()
            .WithMessage("Code is required")
            .Length(codeLength)
            .WithMessage("Code is invalid");
        
        RuleFor(request => request.NewPassword)
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
        
        RuleFor(request => request.NewPasswordConfirmation)
            .NotEmpty()
            .WithMessage("Confirm password is required")
            .Equal(command => command.NewPassword)
            .WithMessage("Passwords do not match");
    }
}