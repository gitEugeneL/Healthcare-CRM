using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Common.Pagination;
using Application.UseCases.MedicalRecords;
using Application.UseCases.MedicalRecords.GetAllMedicalRecords;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.MedicalRecords;

internal sealed record GetAllMedicalRecordsAsManagerQueryParams(
    int? PageNumber,
    int? PageSize,
    Guid? DoctorId,
    Guid? PatientId);

internal sealed class GetAllMedicalRecordsAsManager : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("medical-records/manager", async (
                [AsParameters] GetAllMedicalRecordsAsManagerQueryParams queryParams,
                ISender sender) =>
            {
                var query = new GetAllMedicalRecordsQuery(
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    PatientId: queryParams.PatientId,
                    DoctorId: queryParams.DoctorId);
                
                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
                
            })
            .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(EndpointTags.MedicalRecords)
            .Produces<PaginationResult<MedicalRecordResponse>>();
    }
}