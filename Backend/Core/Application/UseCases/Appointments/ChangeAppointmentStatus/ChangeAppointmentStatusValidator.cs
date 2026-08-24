using Domain.Appointments;
using FluentValidation;

namespace Application.UseCases.Appointments.ChangeAppointmentStatus;

internal sealed class ChangeAppointmentStatusValidator : AbstractValidator<ChangeAppointmentStatusCommand>
{
    private static readonly string[] ValidStatuses = Enum.GetNames(typeof(AppointmentStatus));
    
    public ChangeAppointmentStatusValidator()
    {
        RuleFor(a => a.AppointmentId)
            .NotEmpty()
            .WithMessage("The appointment id is required.");      
        
        RuleFor(a => a.Status)
            .NotEmpty()
            .WithMessage("The status is required.")
            .Must(s => ValidStatuses.Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Status must be one of the following: {string.Join(", ", ValidStatuses)}.");
    }
}