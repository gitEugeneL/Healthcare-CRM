using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.IncludeDoctor;

internal sealed class IncludeDoctorCommandHandler(
    ISpecializationRepository specializationRepository,
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork 
    ) : IRequestHandler<IncludeDoctorCommand, Result<SpecializationResponse>>
{
    public async Task<Result<SpecializationResponse>> Handle(IncludeDoctorCommand command, CancellationToken ct)
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
        
        var includeResult = specialization.IncludeDoctor(doctor);
        if (includeResult.IsFailure)
        {
            return Result.Failure<SpecializationResponse>(includeResult.Error);
        }
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return SpecializationResponse.FromSpecialization(specialization);
    }
}
