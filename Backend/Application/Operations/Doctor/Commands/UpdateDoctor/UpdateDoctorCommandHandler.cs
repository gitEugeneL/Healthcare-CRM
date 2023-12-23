using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using static System.Enum;

namespace Application.Operations.Doctor.Commands.UpdateDoctor;

public class UpdateDoctorCommandHandler(IDoctorRepository doctorRepository) 
    : IRequestHandler<UpdateDoctorCommand, DoctorResponse>
{
    public async Task<DoctorResponse> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.CurrentUserId, cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

        doctor.Status = TryParse<Status>(request.Status, out var result) ? result : doctor.Status;
        doctor.User.FirstName = request.FirstName ?? doctor.User.FirstName;
        doctor.User.LastName = request.LastName ?? doctor.User.LastName;
        doctor.User.Phone = request.Phone ?? doctor.User.Phone;
        doctor.Description = request.Description ?? doctor.Description;
        doctor.Education = request.Education ?? doctor.Education;

        var updatedDoctor = await doctorRepository.UpdateDoctorAsync(doctor, cancellationToken);
        return new DoctorResponse()
            .ToDoctorResponse(updatedDoctor);
    }
}
