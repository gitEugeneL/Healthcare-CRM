using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Doctors.GetAllDoctors;

public sealed record GetAllDoctorsQuery(
    UserAuthRole UserRole,
    bool? IsActive,
    int? PageNumber, 
    int? PageSize, 
    Guid? SpecializationId = null
) : PaginationQuery(PageNumber, PageSize), IRequest<Result<PaginationResult<DoctorResponse>>>;