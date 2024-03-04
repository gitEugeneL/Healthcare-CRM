using Application.Operations.Users.Commands;
using FluentValidation;
using MediatR;

namespace Application.Operations.Patients.Commands.UpdatePatient;

public sealed record UpdatePatientCommand(
    string? Pesel,
    string? DateOfBirth,
    string? Insurance
    ) : UpdateUserCommand, IRequest<PatientResponse>;

public sealed class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientValidator()
    {
        RuleFor(p => p.FirstName)
            .MaximumLength(50);

        RuleFor(p => p.LastName)
            .MaximumLength(100);

        RuleFor(p => p.Phone)
            .MaximumLength(12)
            .Matches("^[+]?\\d+$")
            .WithMessage("Phone number should start with + (optional) and contain only digits.");
        
        RuleFor(p => p.Pesel)
            .Length(11)
            .Matches("^\\d+$")
            .WithMessage("Pesel must be (00000000000)");

        RuleFor(p => p.DateOfBirth)
            .Matches(@"^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$")
            .WithMessage("DateOfBirth must be Y-m-d (1999-12-31)");

        RuleFor(p => p.Insurance)
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
