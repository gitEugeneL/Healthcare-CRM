using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForPatient;

public sealed record GetAllRecordsForPatientQueryPagination(
    bool SortByDate = false,
    bool SortOrderAsc = true,
    int PageNumber = 1,
    int PageSize = 10
) : CurrentUser, ISortResponse, IPaginatedResponse, IRequest<PaginatedList<MedicalRecordResponse>>;
