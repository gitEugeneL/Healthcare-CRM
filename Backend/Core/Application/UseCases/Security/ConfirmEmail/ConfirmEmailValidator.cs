using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Security.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator(IConfiguration configuration)
    {
        var codeLength = int.Parse(configuration["Authentication:ConfirmationCode:Length"] ??
                                   throw new ApplicationException("Code.Length not found in configuration"));
        
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be valid email");

        RuleFor(c => c.ConfirmationCode)
            .NotEmpty()
            .WithMessage("Code is required")
            .Length(codeLength)
            .WithMessage("Code is invalid");
    }
}