using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Common.Pagination;
using Application.UseCases.MedicalRecords;
using Application.UseCases.MedicalRecords.GetAllMedicalRecords;
using MediatR;

namespace Api.Endpoints.MedicalRecords;

internal sealed record GetAllMedicalRecordsAsDoctorQueryParams(
    int? PageNumber,
    int? PageSize,
    Guid? PatientId);

internal sealed class GetMedicalRecordsAsDoctor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("medical-records/doctor", async (
                [AsParameters] GetAllMedicalRecordsAsDoctorQueryParams queryParams,
                HttpContext httpContext,
                ISender sender) =>
            {
                var currentDoctorId = TokenReader.ReadUserIdFromToken(httpContext);
                
                var query = new GetAllMedicalRecordsQuery(
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    PatientId: queryParams.PatientId,
                    DoctorId: currentDoctorId);
                
                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
                
            })
            // TODO .RequireAuthorization(AuthTags.DoctorPolicy)
            .WithTags(ApiTags.MedicalRecords)
            .Produces<PaginationResult<MedicalRecordResponse>>();
    }
}