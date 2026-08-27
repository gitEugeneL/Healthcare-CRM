using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using Domain.MedicalRecords;
using MediatR;

namespace Application.UseCases.MedicalRecords.GetAllMedicalRecords;

internal sealed class GetAllMedicalRecordsQueryHandler(
    IMedicalRecordRepository medicalRecordRepository 
) : IRequestHandler<GetAllMedicalRecordsQuery, Result<PaginationResult<MedicalRecordResponse>>>
{
    public async Task<Result<PaginationResult<MedicalRecordResponse>>> Handle(
        GetAllMedicalRecordsQuery query, 
        CancellationToken ct)
    {
        var (medicalRecords, count) = await medicalRecordRepository.GetAllMedicalRecordsWithPaginationAsync(
            pageNumber: query.NormalizedPageNumber,
            pageSize: query.NormalizedPageSize,
            patientId: query.PatientId,
            doctorId: query.DoctorId,
            ct: ct);

        var response = new PaginationResult<MedicalRecordResponse>(
            Items: medicalRecords.Select(MedicalRecordResponse.FromMedicalRecord).ToList(),
            TotalItems: count,
            PageNumber: query.NormalizedPageNumber,
            PageSize: query.NormalizedPageSize);
        
        return response;
    }
}