using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using MediatR;

namespace Application.UseCases.Doctors.ChangeDoctorStatus;

internal sealed class ChangeDoctorStatusCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ChangeDoctorStatusCommand, Result<DoctorResponse>>
{
    public async Task<Result<DoctorResponse>> Handle(ChangeDoctorStatusCommand command, CancellationToken ct)
    {
        var doctor = await doctorRepository.FindDoctorForChangeStatus(command.DoctorId, ct);

        if (doctor is null)
        {
            return Result.Failure<DoctorResponse>(DoctorErrors.NotFound(command.DoctorId));
        }
        
        if (doctor.WorkSchedule is null)
        {
            return Result.Failure<DoctorResponse>(DoctorErrors.EmptyWorSchedule);
        }
        
        if (doctor.Specializations.Count == 0)
        {
            return Result.Failure<DoctorResponse>(DoctorErrors.EmptySpecializationList);
        }
        
        doctor.ChangeStatus(doctor.Status == DoctorStatus.Active ? DoctorStatus.Disable : DoctorStatus.Active);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return DoctorResponse.FromDoctor(doctor);
    }
}