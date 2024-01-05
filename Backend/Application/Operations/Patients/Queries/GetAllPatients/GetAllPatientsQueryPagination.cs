using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Patients.Queries.GetAllPatients;

public sealed record GetAllPatientsQueryPagination(int PageNumber = 1, int PageSize = 10) 
    : IPaginatedResponse, IRequest<PaginatedList<PatientResponse>>;
