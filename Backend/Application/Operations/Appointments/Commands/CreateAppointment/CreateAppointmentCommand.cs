using Application.Common.Models;
using Application.Operations.Appointments.Validations;
using FluentValidation;
using MediatR;

namespace Application.Operations.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(
    Guid UserDoctorId,
    string Date,
    string StartTime
) : CurrentUser, IRequest<AppointmentResponse>;

public sealed class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(a => a.UserDoctorId)
            .NotEmpty();

        RuleFor(a => a.Date)
            .NotEmpty()
            .Must(DateValidatorAttribute.BeValidDate)
            .WithMessage("Date must be Y-m-d (1999-12-31). "
                         + "Please select a future date and date should not be later than +1 month.");
        
        RuleFor(a => a.StartTime)
            .NotEmpty()
            .Matches("^(?:0[8-9]|1[0-7]):(?:00|15|30|45)$")
            .WithMessage("Incorrect time format (08:00|15|30|45 to 18:00|15|30|45)");
    }
}
