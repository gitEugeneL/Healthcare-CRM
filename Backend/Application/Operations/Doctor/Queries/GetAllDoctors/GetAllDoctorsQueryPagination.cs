using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Doctor.Queries.GetAllDoctors;

public sealed record GetAllDoctorsQueryPagination(
    int PageNumber = 1, 
    int PageSize = 10, 
    Guid? SpecializationId = null
    ) : IPaginatedResponse, IRequest<PaginatedList<DoctorResponse>>;
