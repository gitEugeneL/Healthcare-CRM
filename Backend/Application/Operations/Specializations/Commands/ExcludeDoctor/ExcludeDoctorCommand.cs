using MediatR;

namespace Application.Operations.Specializations.Commands.ExcludeDoctor;

public sealed record ExcludeDoctorCommand : IRequest<SpecializationResponse>
{   
    public Guid UserDoctorId { get; init; }
    public Guid SpecializationId { get; init; }
}
