using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Patients;
using MediatR;

namespace Application.UseCases.Patients.DeactivatePatient;

internal sealed class DeactivatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork   
) : IRequestHandler<DeactivatePatientCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeactivatePatientCommand command, CancellationToken ct)
    {
        var patient = await patientRepository.FindPatientByIdWithTrackingAsync(command.PatientId, ct);
        if (patient is null)
            return Result.Failure<Unit>(PatientErrors.NotFound(command.PatientId));

        if (patient.Status == PatientStatus.Deleted)
            return Result.Failure<Unit>(PatientErrors.AlreadyDeactivated(command.PatientId));
        
        patient.Deactivate();

        await unitOfWork.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}