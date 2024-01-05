using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Offices.Commands.UpdateOffice;

public sealed record UpdateOfficeCommand(
    [Required]
    Guid OfficeId, 
    
    [Required]
    string Name
) : IRequest<OfficeResponse>;
