using System.ComponentModel.DataAnnotations;
using Application.Operations.Users.Commands;
using MediatR;

namespace Application.Operations.Doctor.Commands.UpdateDoctor;

public sealed record UpdateDoctorCommand : UpdateUserCommand, IRequest<DoctorResponse>
{
    [AllowedValues(
        "Active", "Disable", 
        ErrorMessage = "Available value: 'Active' or 'Disable")
    ]
    public string? Status { get; init; }
    
    [MaxLength(200)]
    public string? Description { get; init; }
    
    [MaxLength(200)]
    public string? Education { get; init; }
}
