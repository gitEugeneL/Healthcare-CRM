using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using MediatR;

namespace Application.UseCases.Doctors.UpdateDoctor;

internal sealed class UpdateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork   
) : IRequestHandler<UpdateDoctorCommand, Result<DoctorResponse>>
{
    public async Task<Result<DoctorResponse>> Handle(UpdateDoctorCommand command, CancellationToken ct)
    {
        var doctor = await doctorRepository.FindDoctorByIdWithTrackingAsync(command.DoctorId, ct);
        if (doctor is null)
            return Result.Failure<DoctorResponse>(DoctorErrors.NotFound(command.DoctorId));
        
        if (command.FirstName is not null)
            doctor.User.ChangeFirstName(command.FirstName);

        if (command.LastName is not null)
            doctor.User.ChangeLastName(command.LastName);

        if (command.Phone is not null)
            doctor.User.ChangePhone(command.Phone);

        if (command.Description is not null)
            doctor.UpdateDescription(command.Description);

        if (command.Education is not null)
            doctor.UpdateEducation(command.Education);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return DoctorResponse.FromDoctor(doctor);
    }
}
