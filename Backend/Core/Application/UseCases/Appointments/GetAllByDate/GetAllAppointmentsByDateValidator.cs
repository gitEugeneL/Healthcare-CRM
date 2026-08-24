using FluentValidation;

namespace Application.UseCases.Appointments.GetAllByDate;

internal sealed class GetAllAppointmentsByDateValidator : AbstractValidator<GetAllAppointmentsByDateQuery>
{
    public GetAllAppointmentsByDateValidator()
    {
        RuleFor(a => a.Date)
            .NotEmpty()
            .WithMessage("The date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddMonths(3)))
            .WithMessage("The date must not be more than three months in the future.");
    }
}