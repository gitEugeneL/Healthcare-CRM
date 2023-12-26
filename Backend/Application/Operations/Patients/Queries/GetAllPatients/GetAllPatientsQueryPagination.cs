using Application.Common.Models;
using MediatR;

namespace Application.Operations.Patients.Queries.GetAllPatients;

public sealed record GetAllPatientsQueryPagination : PaginatedResponse, IRequest<PaginatedList<PatientResponse>>;
