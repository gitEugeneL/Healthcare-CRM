using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.ExcludeDoctor;

public sealed record ExcludeDoctorCommand(
    Guid SpecializationId,
    Guid DoctorId
) : IRequest<Result<SpecializationResponse>>;
