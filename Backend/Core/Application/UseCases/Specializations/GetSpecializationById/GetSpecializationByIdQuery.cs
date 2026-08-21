using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.GetSpecializationById;

public sealed record GetSpecializationByIdQuery(
    Guid SpecializationId
) : IRequest<Result<SpecializationResponse>>;