using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Patients.Queries.GetAllPatients;

public class GetAllPatientsQueryHandler(IPatientRepository patientRepository) 
    : IRequestHandler<GetAllPatientsQueryPagination, PaginatedList<PatientResponse>>
{
    public async Task<PaginatedList<PatientResponse>> 
        Handle(GetAllPatientsQueryPagination request, CancellationToken cancellationToken)
    {
        var (patients, count) = await patientRepository.GetPatientsWithPaginationAsync(
            cancellationToken: cancellationToken,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
        );

        var patientResponses = patients
            .Select(patient => new PatientResponse().ToPatientResponse(patient))
            .ToList();

        return new PaginatedList<PatientResponse>(patientResponses, count, request.PageNumber, request.PageSize);
    }
}
