using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Common.Pagination;
using Application.UseCases.MedicalRecords;
using Application.UseCases.MedicalRecords.GetAllMedicalRecords;
using MediatR;
using Security.Setups;
using Security.Utils;

namespace Api.Endpoints.MedicalRecords;

internal sealed record GetAllMedicalRecordsAsPatientQueryParams(
    int? PageNumber,
    int? PageSize,
    Guid? DoctorId);

internal sealed class GetAllMedicalRecordsAsPatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("medical-records/patient", async (
                [AsParameters] GetAllMedicalRecordsAsPatientQueryParams queryParams,
                HttpContext httpContext,
                ISender sender) =>
            {
                var currentPatientId = TokenReader.ReadUserIdFromToken(httpContext);
                
                var query = new GetAllMedicalRecordsQuery(
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    PatientId: currentPatientId,
                    DoctorId: queryParams.DoctorId);
                
                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
                
            })
            .RequireAuthorization(AuthTags.PatientPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.MedicalRecords)
            .Produces<PaginationResult<MedicalRecordResponse>>();
    }
}