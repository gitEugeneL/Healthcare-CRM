using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Patients;
using MediatR;

namespace Application.UseCases.Patients.UpdatePatient;

internal sealed class UpdatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)    
: IRequestHandler<UpdatePatientCommand, Result<PatientResponse>>
{
    public async Task<Result<PatientResponse>> Handle(UpdatePatientCommand command, CancellationToken ct)
    {
        var patient = await patientRepository.FindPatientByIdWithTrackingAsync(command.PatientId, ct);
        if (patient is null)
            return Result.Failure<PatientResponse>(PatientErrors.NotFound(command.PatientId));

        if (command.FirstName is not null)
            patient.User.ChangeFirstName(command.FirstName);

        if (command.LastName is not null)
            patient.User.ChangeLastName(command.LastName);

        if (command.Phone is not null)
            patient.User.ChangePhone(command.Phone);

        if (command.Pesel is not null)
            patient.ChangePesel(command.Pesel);

        if (command.DateOfBirth is not null)
            patient.ChangeDateOfBirth(command.DateOfBirth.Value);
        
        if (command.Insurance is not null)
            patient.ChangeInsurance(command.Insurance);

        if (command.Address.Province is not null)
            patient.Address.ChangeProvince(command.Address.Province);

        if (command.Address.PostalCode is not null)
            patient.Address.ChangePostalCode(command.Address.PostalCode);

        if (command.Address.City is not null)
            patient.Address.ChangeCity(command.Address.City);
        
        if (command.Address.Street is not null)
            patient.Address.ChangeStreet(command.Address.Street);

        if (command.Address.Hose is not null)
            patient.Address.ChangeHose(command.Address.Hose);

        if (command.Address.Apartment is not null)
            patient.Address.ChangeApartment(command.Address.Apartment);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return PatientResponse.FromPatient(patient);
    }
}