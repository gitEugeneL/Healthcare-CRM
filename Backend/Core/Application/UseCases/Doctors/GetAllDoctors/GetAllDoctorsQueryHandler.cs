using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Doctors.GetAllDoctors;

internal sealed class GetAllDoctorsQueryHandler(
    IDoctorRepository doctorRepository
) : IRequestHandler<GetAllDoctorsQuery, Result<PaginationResult<DoctorResponse>>>
{
    public async Task<Result<PaginationResult<DoctorResponse>>> Handle(GetAllDoctorsQuery query, CancellationToken ct)
    {
        DoctorStatus? status = query.UserRole != UserAuthRole.Manager
            ? DoctorStatus.Active
            : query.IsActive switch
            {
                true => DoctorStatus.Active,
                false => DoctorStatus.Disable,
                null => null
            };
        
        var (doctors, count) = await doctorRepository.GetDoctorsWithPaginationAsync(
            pageNumber: query.NormalizedPageNumber,
            pageSize: query.NormalizedPageSize,
            specializationId: query.SpecializationId,
            status: status,
            ct: ct
        );
        
        var response = new PaginationResult<DoctorResponse>(
            Items: doctors.Select(DoctorResponse.FromDoctor).ToList(),
            TotalItems: count,
            PageNumber: query.NormalizedPageNumber,
            PageSize: query.NormalizedPageSize
        );

        return response;
    }
}
