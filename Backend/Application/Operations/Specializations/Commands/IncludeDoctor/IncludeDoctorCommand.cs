using MediatR;

namespace Application.Operations.Specializations.Commands.IncludeDoctor;

public sealed record IncludeDoctorCommand : IRequest<SpecializationResponse>
{
    public Guid UserDoctorId { get; init; }
    public Guid SpecializationId { get; init; }
}
