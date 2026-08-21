using Domain.Abstractions.Errors;
using Domain.Patients;
using MediatR;

namespace Application.UseCases.Patients.GetPatientById;

internal sealed class GetPatientByIdQueryHandler(
    IPatientRepository patientRepository    
) : IRequestHandler<GetPatientByIdQuery, Result<PatientResponse>>
{
    public async Task<Result<PatientResponse>> Handle(GetPatientByIdQuery query, CancellationToken ct)
    {
        var patient = await patientRepository.FindPatientByIdAsync(query.PatientId, ct);
        
        return patient is null
            ? Result.Failure<PatientResponse>(PatientErrors.NotFound(query.PatientId)) 
            : PatientResponse.FromPatient(patient);
    }
}