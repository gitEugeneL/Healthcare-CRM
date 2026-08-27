using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Common.Pagination;
using Application.UseCases.Doctors;
using Application.UseCases.Doctors.GetAllDoctors;
using Domain.Abstractions.Errors;
using Domain.Users;
using MediatR;

namespace Api.Endpoints.Doctors;

internal sealed record GetAllDoctorsQueryParams(
    Guid? SpecializationId,
    int? PageNumber,
    int? PageSize,
    bool? IsActive
);

internal class GetAllDoctors : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("doctors", async (
                [AsParameters] GetAllDoctorsQueryParams queryParams,
                HttpContext httpContext,
                ISender sender) =>
            {
                var authRole = Enum.Parse<UserAuthRole>(TokenReader.ReadUserRoleFromToken(httpContext));
                
                var query = new GetAllDoctorsQuery(
                    UserRole: authRole,
                    SpecializationId: queryParams.SpecializationId,
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    IsActive: queryParams.IsActive);
                
                Result<PaginationResult<DoctorResponse>> result = await sender.Send(query); 
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // todo .RequireAuthorization(AuthTags.ManagerOrPatientPolicy)
            .WithTags(ApiTags.Doctors)
            .Produces<PaginationResult<DoctorResponse>>();
    }
}