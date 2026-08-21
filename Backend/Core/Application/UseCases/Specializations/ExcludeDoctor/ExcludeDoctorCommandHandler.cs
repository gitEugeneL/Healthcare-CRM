using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.ExcludeDoctor;

internal sealed class ExcludeDoctorCommandHandler(
    ISpecializationRepository specializationRepository,
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ExcludeDoctorCommand, Result<SpecializationResponse>>
{
    public async Task<Result<SpecializationResponse>> Handle(ExcludeDoctorCommand command, CancellationToken ct)
    {
        var specialization = await specializationRepository
            .FindSpecializationByIdWithDoctorsAndWithTrackingAsync(command.SpecializationId, ct);

        if (specialization is null)
        {
            return Result.Failure<SpecializationResponse>(SpecializationErrors.NotFound(command.SpecializationId));
        }
        
        var doctor = await doctorRepository.FindDoctorByIdWithTrackingAsync(command.DoctorId, ct);
        if (doctor is null)
        {
            return Result.Failure<SpecializationResponse>(DoctorErrors.NotFound(command.DoctorId));
        }
        
        var excludeResult = specialization.ExcludeDoctor(doctor);
        if (excludeResult.IsFailure)
        {
            return Result.Failure<SpecializationResponse>(excludeResult.Error);
        }
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return SpecializationResponse.FromSpecialization(specialization);
        
    }
}
