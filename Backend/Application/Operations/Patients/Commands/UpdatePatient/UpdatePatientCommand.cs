using System.ComponentModel.DataAnnotations;
using Application.Operations.Users.Commands;
using MediatR;

namespace Application.Operations.Patients.Commands.UpdatePatient;

public sealed record UpdatePatientCommand : UpdateUserCommand, IRequest<PatientResponse>
{
    [MaxLength(11)]
    [MinLength(11)]
    [RegularExpression(
        "^\\d+$",
        ErrorMessage = "Pesel must be (00000000000)"
    )]
    public string? Pesel { get; set; }
    
    [RegularExpression(
        "^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$",
        ErrorMessage = "DateOfBirth must be Y-m-d (1999-12-31)"
    )]
    public string? DateOfBirth { get; set; }
    
    [MinLength(3)]
    [MaxLength(100)]
    public string? Insurance { get; set; }
}
