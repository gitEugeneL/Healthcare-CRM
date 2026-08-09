using Domain.WorkSchedules;
using FluentValidation;
namespace Application.UseCases.WorkSchedules.ConfigWorkSchedule;

internal sealed class ConfigWorkScheduleValidator : AbstractValidator<ConfigConfigWorkScheduleCommand>
{
    private static readonly TimeOnly MinTime = new(6, 0);
    private static readonly TimeOnly MaxTime = new(20, 0);

    public ConfigWorkScheduleValidator()
    {
        RuleFor(c => c.DoctorId)
            .NotEmpty()
            .WithMessage("The doctor id is required.");

        RuleFor(c => c.StartTime)
            .InclusiveBetween(MinTime, MaxTime)
            .WithMessage($"StartTime must be between {MinTime:HH:mm} and {MaxTime:HH:mm}");

        RuleFor(c => c.EndTime)
            .InclusiveBetween(MinTime, MaxTime)
            .WithMessage($"EndTime must be between {MinTime:HH:mm} and {MaxTime:HH:mm}")
            .GreaterThan(c => c.StartTime)
            .WithMessage("EndTime must be greater than StartTime");

        RuleFor(c => c.AppointmentDuration)
            .Must(i => Enum.IsDefined(typeof(AppointmentDuration), i))
            .WithMessage($"Interval must be one of: {string
                .Join(", ", Enum.GetValues<AppointmentDuration>().Select(d => (int)d))} (minutes)");

        RuleFor(c => c)
            .Must(c => IsDivisible(c.StartTime, c.EndTime, c.AppointmentDuration))
            .WithMessage("The time range must be evenly divisible by the interval")
            .OverridePropertyName("Interval")
            .When(c => Enum.IsDefined(typeof(AppointmentDuration), c.AppointmentDuration));

        RuleFor(c => c.Workdays)
            .NotEmpty()
            .WithMessage("At least one workday must be specified.")
            .Must(w => w.All(d => Enum.IsDefined(typeof(Workday), d)))
            .WithMessage($"Workdays must contain values between {(int)Workday.Monday} and {(int)Workday.Sunday}.")
            .Must(w => w.Length == w.Distinct().Count())
            .WithMessage("Workdays must not contain duplicate values.");
    }

    private static bool IsDivisible(TimeOnly start, TimeOnly end, int interval)
    {
        var totalMinutes = (end - start).TotalMinutes;
        return totalMinutes > 0 && totalMinutes % interval == 0;
    }
}