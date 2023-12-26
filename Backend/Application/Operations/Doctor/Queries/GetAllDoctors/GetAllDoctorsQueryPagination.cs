using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Doctor.Queries.GetAllDoctors;

public sealed record GetAllDoctorsQueryPagination : PaginatedResponse, IRequest<PaginatedList<DoctorResponse>>
{
    public Guid? SpecializationId { get; init; }
}
