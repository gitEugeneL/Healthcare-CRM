using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Doctor.Queries.GetAllDoctors;

public class GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository)
    : IRequestHandler<GetAllDoctorsQueryPagination, PaginatedList<DoctorResponse>>
{
    public async Task<PaginatedList<DoctorResponse>> 
        Handle(GetAllDoctorsQueryPagination request, CancellationToken cancellationToken)
    {
        var (doctors, count) = await doctorRepository.GetDoctorsWithPaginationAsync(
            cancellationToken: cancellationToken,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            specializationId: request.SpecializationId
        );
        
        var doctorResponses = doctors
            .Select(doctor => new DoctorResponse().ToDoctorResponse(doctor))
            .ToList();

        return new PaginatedList<DoctorResponse>(doctorResponses, count, request.PageNumber, request.PageSize);
    }
}
