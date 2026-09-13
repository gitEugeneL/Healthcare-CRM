using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Common.Pagination;
using Application.UseCases.Patients;
using Application.UseCases.Patients.GetAllPatients;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Patients;

internal sealed record GetAllPatientsQueryParams(
    int? PageNumber,
    int? PageSize,
    Guid? DoctorId);

internal sealed class GetAllPatients : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("patients", async (
            [AsParameters] GetAllPatientsQueryParams queryParams,
            ISender sender) =>
        {
            var query = new GetAllPatientsQuery(
                PageNumber: queryParams.PageNumber,
                PageSize: queryParams.PageSize,
                DoctorId: queryParams.DoctorId);
            
            var result = await sender.Send(query);
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .RequireAuthorization(AuthTags.ManagerOrPatientPolicy)
        .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
        .WithTags(EndpointTags.Patients)
        .Produces<PaginationResult<PatientResponse>>();
    }
}