using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Patients.Commands.DeletePatient;

public class DeletePatientCommandHandler(IPatientRepository patientRepository)
    : IRequestHandler<DeletePatientCommand, Unit>
{
    public async Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.FindPatientByUserIdAsync(request.GetCurrentUserId(), cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());

        await patientRepository.DeletePatientAsync(patient, cancellationToken);
        return await Unit.Task;
    }
}
