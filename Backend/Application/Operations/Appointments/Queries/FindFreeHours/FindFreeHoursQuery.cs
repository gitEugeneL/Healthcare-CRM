using System.ComponentModel.DataAnnotations;
using Application.Operations.Appointments.Validations;
using MediatR;

namespace Application.Operations.Appointments.Queries.FindFreeHours;

public sealed record FindFreeHoursQuery : IRequest<FreeHoursResponse>
{
    [Required]
    public required Guid UserDoctorId { get; init; }
    
    [Required]
    [DateValidation]
    public required string Date { get; init; }
}
