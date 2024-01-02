using System.ComponentModel.DataAnnotations;
using Application.Operations.Appointment.Validations;
using MediatR;

namespace Application.Operations.Appointment.Queries.FindFreeHours;

public sealed record FindFreeHoursQuery : IRequest<FreeHoursResponse>
{
    [Required]
    public required Guid UserDoctorId { get; init; }
    
    [Required]
    [DateValidation]
    public required string Date { get; init; }
}
