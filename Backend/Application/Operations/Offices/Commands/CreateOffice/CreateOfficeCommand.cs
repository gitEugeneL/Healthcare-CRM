using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Offices.Commands.CreateOffice;

public record CreateOfficeCommand(
    [Required]
    [MaxLength(50)]
    string Name,
    
    [Required]
    [Range(1, 999)]
    ushort Number
) : IRequest<OfficeResponse>;