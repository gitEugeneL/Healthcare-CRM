using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.MedicalRecords.GetAllMedicalRecords;

public sealed record GetAllMedicalRecordsQuery(
    int? PageNumber, 
    int? PageSize,
    Guid? DoctorId,
    Guid? PatientId 
) : PaginationQuery(PageNumber, PageSize), IRequest<Result<PaginationResult<MedicalRecordResponse>>>;