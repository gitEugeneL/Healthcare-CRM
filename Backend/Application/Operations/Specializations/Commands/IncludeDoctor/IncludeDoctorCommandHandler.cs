using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Specializations.Commands.IncludeDoctor;

public class IncludeDoctorCommandHandler(
    ISpecializationRepository specializationRepository,
    IDoctorRepository doctorRepository
    )
    : IRequestHandler<IncludeDoctorCommand, SpecializationResponse>
{
    public async Task<SpecializationResponse> Handle(IncludeDoctorCommand request, CancellationToken cancellationToken)
    {
        var specialization = await specializationRepository
                                 .FindSpecializationByIdAsync(request.SpecializationId, cancellationToken) 
                             ?? throw new NotFoundException(nameof(Specialization), request.SpecializationId);

        var doctor = await doctorRepository
                         .FindDoctorByUserIdAsync(request.UserDoctorId, cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.UserDoctorId);

        if (specialization.UserDoctors.Contains(doctor))
            throw new AlreadyExistException(nameof(User), request.UserDoctorId);

        specialization.UserDoctors.Add(doctor);

        var updatedSpecialization = await specializationRepository
            .UpdateSpecializationAsync(specialization, cancellationToken);

        return new SpecializationResponse()
            .ToSpecializationResponse(updatedSpecialization);
    }
}
