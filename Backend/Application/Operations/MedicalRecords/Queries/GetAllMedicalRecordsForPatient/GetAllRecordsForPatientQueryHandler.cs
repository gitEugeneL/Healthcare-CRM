using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForPatient;

public class GetAllRecordsForPatientQueryHandler(
    IMedicalRecordRepository medicalRecordRepository,
    IPatientRepository patientRepository
    ) : IRequestHandler<GetAllRecordsForPatientQueryPagination, PaginatedList<MedicalRecordResponse>>
{
    public async Task<PaginatedList<MedicalRecordResponse>> 
        Handle(GetAllRecordsForPatientQueryPagination request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.FindPatientByUserIdAsync(request.GetCurrentUserId(), cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());
        
        var (medicalRecords, count) = await medicalRecordRepository
            .GetMedicalRecordsForPatientWithPaginationAsync(
                cancellationToken: cancellationToken,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                patientId: patient.User.Id,
                sortByDate: request.SortByDate,
                sortOrderAsc: request.SortOrderAsc
            );

        var medicalRecordResponses = medicalRecords
            .Select(mr => new MedicalRecordResponse()
                .ToMedicalRecordResponse(mr))
            .ToList();

        return new PaginatedList<MedicalRecordResponse>(
            medicalRecordResponses, count, request.PageNumber, request.PageSize);
    }
}
