using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Operations.Appointments.Validations;
using MediatR;

namespace Application.Operations.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand : CurrentUser, IRequest<AppointmentResponse>
{
    [Required]
    public required Guid UserDoctorId { get; init; }
    
    [Required]
    [DateValidation]
    public required string Date { get; init; }

    [Required]
    [RegularExpression(
        "^(?:0[8-9]|1[0-7]):(?:00|15|30|45)$",
        ErrorMessage = "Incorrect time format (08:00|15|30|45 to 18:00|15|30|45)"
    )]
    public required string StartTime { get; init; }
}
