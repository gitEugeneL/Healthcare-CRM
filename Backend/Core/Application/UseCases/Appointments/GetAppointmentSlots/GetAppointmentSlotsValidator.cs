using FluentValidation;

namespace Application.UseCases.Appointments.GetAppointmentSlots;

internal sealed class GetAppointmentSlotsValidator : AbstractValidator<GetAppointmentSlotsQuery>
{
    public GetAppointmentSlotsValidator()
    {
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
    }
}