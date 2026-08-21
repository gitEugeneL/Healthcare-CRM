using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.DeleteSpecialization;

public sealed record DeleteSpecializationCommand(Guid SpecializationId) : IRequest<Result<Unit>>;
