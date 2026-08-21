using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Patients.CreatePatient;

internal sealed class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        Include(new CreateUserValidator());
        
        RuleFor(p => p.Insurance)
            .MaximumLength(250)
            .WithMessage("The insurance must be less than 250 characters.");
        
        RuleFor(p => p.Pesel)
            .NotEmpty()
            .WithMessage("The pesel is required.")
            .Matches(@"^[0-9]{11}$")
            .WithMessage("The pesel must be 11 digits.");
            
        RuleFor(p => p.DateOfBirth)
            .NotEmpty()
            .WithMessage("The date of birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date of birth must be in the past.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
            .WithMessage("The date of birth must be within the last 120 years.");
        
        RuleFor(p => p.Address.Province)
            .NotEmpty()
            .WithMessage("The province is required.")
            .MaximumLength(100)
            .WithMessage("The province must be less than 100 characters.");
        
        RuleFor(p => p.Address.City)
            .NotEmpty()
            .WithMessage("The city is required.")
            .MaximumLength(100)
            .WithMessage("The city must be less than 100 characters.");       
        
        RuleFor(p => p.Address.Street)
            .NotEmpty()
            .WithMessage("The street is required.")
            .MaximumLength(100)
            .WithMessage("The street must be less than 100 characters.");       
        
        RuleFor(p => p.Address.Hose)
            .NotEmpty()
            .WithMessage("The house number is required.")
            .MaximumLength(10)
            .WithMessage("The house number must be less than 10 characters.");

        RuleFor(p => p.Address.Apartment)
            .MaximumLength(10)
            .WithMessage("The apartment number must be less than 10 characters.");

        RuleFor(p => p.Address.PostalCode)
            .NotEmpty()
            .WithMessage("The postal code is required.")
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("The postal code must be in polish format XX-XXX.");
    }
}