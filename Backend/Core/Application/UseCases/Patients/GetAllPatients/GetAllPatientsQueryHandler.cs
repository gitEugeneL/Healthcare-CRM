using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using Domain.Patients;
using MediatR;

namespace Application.UseCases.Patients.GetAllPatients;

internal sealed class GetAllPatientQueryHandler(
    IPatientRepository patientRepository
) : IRequestHandler<GetAllPatientsQuery, Result<PaginationResult<PatientResponse>>>
{
    public async Task<Result<PaginationResult<PatientResponse>>> Handle(GetAllPatientsQuery query, CancellationToken ct)
    {
        var (patients, count) = await patientRepository.GetAllPatientsWithPaginationAsync(
            pageNumber: query.NormalizedPageNumber,
            pageSize: query.NormalizedPageSize,
            doctorId: query.DoctorId,
            ct: ct);

        var response = new PaginationResult<PatientResponse>(
            Items: patients.Select(PatientResponse.FromPatient).ToList(),
            TotalItems: count,
            PageNumber: query.NormalizedPageNumber,
            PageSize: query.NormalizedPageSize);

        return response;
    }
}