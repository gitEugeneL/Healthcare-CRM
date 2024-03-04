using Application.Common.Models;
using Application.Operations.AppointSettings.Validations;
using FluentValidation;
using MediatR;

namespace Application.Operations.AppointSettings.Commands.Config;

public sealed record ConfigAppointmentCommand(
    string? StartTime,
    string? EndTime,
    string? Interval,
    int[]? Workdays
) : CurrentUser, IRequest<AppointmentSettingsResponse>;

public sealed class ConfigAppointmentValidator : AbstractValidator<ConfigAppointmentCommand>
{
    public ConfigAppointmentValidator()
    {
        RuleFor(c => c.StartTime)
            .Matches("^(0[8-9]|1[0-7]):00$")
            .WithMessage("Incorrect time format. Available: (08:00 to 17:00)");

        RuleFor(c => c.EndTime)
            .Matches("^(0[9]|1[0-8]):00$")
            .WithMessage("Incorrect time format. Available: (09:00 to 18:00)");

        RuleFor(c => c.Interval)
            .Matches("^(60min|15min|30min)$")
            .WithMessage("Incorrect interval. Available: '60min' or '15min' or '30min'");

        RuleFor(c => c.Workdays)
            .Must(WorkdaysValidationAttribute.BeValidWorkdays)
            .WithMessage("Workdays must contain values between 1 and 7");

    }
}



