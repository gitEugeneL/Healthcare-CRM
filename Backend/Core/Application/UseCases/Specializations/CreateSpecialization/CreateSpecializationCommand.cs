using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.CreateSpecialization;

public sealed record CreateSpecializationCommand(
    string Name,
    string? Description
) : IRequest<Result<SpecializationResponse>>;