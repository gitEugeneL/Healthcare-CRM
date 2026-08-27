using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Common.Pagination;
using Application.UseCases.Patients;
using Application.UseCases.Patients.GetAllPatients;
using MediatR;

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
        // TODO .RequireAuthorization(AuthTags.ManagerOrPatientPolicy)
        .WithTags(ApiTags.Patients)
        .Produces<PaginationResult<PatientResponse>>();
    }
}