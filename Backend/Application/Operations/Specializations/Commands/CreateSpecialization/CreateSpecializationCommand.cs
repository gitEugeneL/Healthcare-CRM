using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Specializations.Commands.CreateSpecialization;

public sealed class CreateSpecializationCommand : IRequest<SpecializationResponse>
{
    [Required]
    [MaxLength(100)]
    public required string Value { get; init; }

    [MaxLength(200)] 
    public string? Description { get; init; }
}
