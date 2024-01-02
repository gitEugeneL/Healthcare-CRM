using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Operations.AppointSettings.Validations;
using MediatR;

namespace Application.Operations.AppointSettings.Commands.Config;

public sealed record ConfigAppointmentCommand : CurrentUser, IRequest<AppointmentSettingsResponse>
{
    [RegularExpression(
        "^(0[8-9]|1[0-7]):00$",
        ErrorMessage = "Incorrect time format. Available: (08:00 to 17:00)"
    )]
    public string? StartTime { get; init; }
    
    [RegularExpression(
        "^(0[9]|1[0-8]):00$",
        ErrorMessage = "Incorrect time format. Available: (09:00 to 18:00)"
    )]
    public string? EndTime { get; init; }
    
    [RegularExpression(
        "^(60min|15min|30min)$",
        ErrorMessage = "Incorrect interval. Available: '60min' or '15min' or '30min'"
    )]
    public string? Interval { get; init; }

    [WorkdaysValidation(
        ErrorMessage = "Workdays must contain values between 1 and 7"
    )]
    public int[]? Workdays { get; init; }
}

