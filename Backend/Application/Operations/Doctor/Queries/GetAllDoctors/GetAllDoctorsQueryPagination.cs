using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Doctor.Queries.GetAllDoctors;

public sealed record GetAllDoctorsQueryPagination : IRequest<PaginatedList<DoctorResponse>>
{
    public Guid? SpecializationId { get; init; }
   
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;
    
    [Range(5, 30)]
    public int PageSize { get; init; } = 10; 
}
