using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Patients.GetAllPatients;

public sealed record GetAllPatientsQuery(
    Guid? DoctorId,
    int? PageNumber, 
    int? PageSize
) : PaginationQuery(PageNumber, PageSize), IRequest<Result<PaginationResult<PatientResponse>>>;