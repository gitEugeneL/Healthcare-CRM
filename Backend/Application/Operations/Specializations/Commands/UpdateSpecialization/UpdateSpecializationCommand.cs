using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Specializations.Commands.UpdateSpecialization;

public sealed record UpdateSpecializationCommand : IRequest<SpecializationResponse>
{
    [Required]
    public Guid SpecializationId { get; init; }

    [Required]
    [MaxLength(200)]
    public required string Description { get; init; }
}
