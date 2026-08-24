using FluentValidation;

namespace Application.UseCases.Appointments.CreateAppointment;

internal sealed class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(a => a.PatientId)
            .NotEmpty()
            .WithMessage("The patient id is required.");       
        
        RuleFor(a => a.DoctorId)
            .NotEmpty()
            .WithMessage("The doctor id is required.");
        
        RuleFor(a => a.Date)
            .NotEmpty()
            .WithMessage("The date is required.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
            .WithMessage("The date must be at least tomorrow.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddMonths(3)))
            .WithMessage("The date must not be more than three months in the future.");

        RuleFor(a => a.StartTime)
            .NotEmpty()
            .WithMessage("The start time is required.");
    }
}