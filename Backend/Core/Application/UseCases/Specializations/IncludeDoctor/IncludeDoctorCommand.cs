using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.IncludeDoctor;

public sealed record IncludeDoctorCommand(
    Guid SpecializationId,
    Guid DoctorId
) : IRequest<Result<SpecializationResponse>>;