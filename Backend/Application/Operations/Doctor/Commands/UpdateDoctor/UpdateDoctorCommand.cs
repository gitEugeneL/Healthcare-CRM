using Application.Operations.Users.Commands;
using FluentValidation;
using MediatR;

namespace Application.Operations.Doctor.Commands.UpdateDoctor;

public sealed record UpdateDoctorCommand(
    string? Status,
    string? Description,
    string? Education
) : UpdateUserCommand, IRequest<DoctorResponse>;

public sealed class UpdateDoctorValidator : AbstractValidator<UpdateDoctorCommand>
{
    public UpdateDoctorValidator()
    {
        RuleFor(d => d.FirstName)
            .MaximumLength(50);

        RuleFor(d => d.LastName)
            .MaximumLength(100);

        RuleFor(d => d.Phone)
            .MaximumLength(12)
            .Matches("^[+]?\\d+$")
            .WithMessage("Phone number should start with + (optional) and contain only digits.");

        RuleFor(d => d.Status)
            .Must(x => x is "Active" or "Disable")
            .WithMessage("Available value: 'Active' or 'Disable");

        RuleFor(d => d.Description)
            .MaximumLength(200);

        RuleFor(d => d.Education)
            .MaximumLength(200);
    }
}