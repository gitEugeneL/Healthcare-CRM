using Application.Operations.Users.Commands;
using FluentValidation;
using MediatR;

namespace Application.Operations.Patients.Commands.CreatePatient;

public sealed record CreatePatientCommand : CreateUserCommand, IRequest<PatientResponse>;

public sealed class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
            .WithMessage("The password must contain at least one letter, one special character, and one digit.");
    }
}