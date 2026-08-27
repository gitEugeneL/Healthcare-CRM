using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Common.Pagination;
using Application.UseCases.MedicalRecords;
using Application.UseCases.MedicalRecords.GetAllMedicalRecords;
using MediatR;

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
        // TODO .RequireAuthorization(AuthTags.PatientPolicy)
        .WithTags(ApiTags.MedicalRecords)
        .Produces<PaginationResult<MedicalRecordResponse>>();
    }
}