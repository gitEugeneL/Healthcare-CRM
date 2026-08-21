using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Patients.UpdatePatient;

internal sealed class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientValidator()
    {
        Include(new UpdateUserValidator());
        
        RuleFor(p => p.Insurance)
            .MaximumLength(250)
            .WithMessage("The insurance must be less than 250 characters.");

        RuleFor(p => p.Pesel)
            .Matches(@"^[0-9]{11}$")
            .WithMessage("The pesel must be 11 digits.");
        
        RuleFor(p => p.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date of birth must be in the past.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
            .WithMessage("The date of birth must be within the last 120 years.");

        RuleFor(p => p.Address.Province)
            .MaximumLength(100)
            .WithMessage("The province must be less than 100 characters.");
        
        RuleFor(p => p.Address.City)
            .MaximumLength(100)
            .WithMessage("The city must be less than 100 characters.");       
        
        RuleFor(p => p.Address.Street)
            .MaximumLength(100)
            .WithMessage("The street must be less than 100 characters.");       
        
        RuleFor(p => p.Address.Hose)
            .MaximumLength(10)
            .WithMessage("The house number must be less than 10 characters.");

        RuleFor(p => p.Address.Apartment)
            .MaximumLength(10)
            .WithMessage("The apartment number must be less than 10 characters.");

        RuleFor(p => p.Address.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("The postal code must be in polish format XX-XXX.");
    }
}