using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Patients.Queries.GetPatient;

public class GetPatientQueryHandler(IPatientRepository patientRepository)
    : IRequestHandler<GetPatientQuery, PatientResponse>
{
    public async Task<PatientResponse> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.FindPatientByUserIdAsync(request.Id, cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.Id);

        return new PatientResponse()
            .ToPatientResponse(patient);
    }
}
