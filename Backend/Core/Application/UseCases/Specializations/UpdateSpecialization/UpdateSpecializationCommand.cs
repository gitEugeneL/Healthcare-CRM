using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.UpdateSpecialization;

public sealed record UpdateSpecializationCommand(
    Guid SpecializationId,
    string Description
) : IRequest<Result<SpecializationResponse>>;