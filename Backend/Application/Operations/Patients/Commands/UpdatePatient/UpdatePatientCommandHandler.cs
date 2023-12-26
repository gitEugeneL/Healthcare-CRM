using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Patients.Commands.UpdatePatient;

public class UpdatePatientCommandHandler(IPatientRepository patientRepository) 
    : IRequestHandler<UpdatePatientCommand, PatientResponse>
{
    public async Task<PatientResponse> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.FindPatientByUserIdAsync(request.CurrentUserId, cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

        if (request.DateOfBirth is not null)
            patient.DateOfBirth = DateOnly.FromDateTime(DateTime.Parse(request.DateOfBirth));
        
        patient.User.FirstName = request.FirstName ?? patient.User.FirstName;
        patient.User.LastName = request.LastName ?? patient.User.LastName;
        patient.User.Phone = request.Phone ?? patient.User.Phone;
        patient.Pesel = request.Pesel ?? patient.Pesel;
        patient.Insurance = request.Insurance ?? patient.Insurance;

        var updatedPatient = await patientRepository.UpdatePatientAsync(patient, cancellationToken);
        return new PatientResponse().ToPatientResponse(updatedPatient);
    }
}
