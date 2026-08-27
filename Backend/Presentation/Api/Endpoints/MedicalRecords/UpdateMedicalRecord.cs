using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.MedicalRecords;
using Application.UseCases.MedicalRecords.UpdateMedicalRecord;
using MediatR;

namespace Api.Endpoints.MedicalRecords;

internal sealed record UpdateMedicalRecordRequest(
    string? Title,
    string? RecommendationForPatient,
    DateOnly? FollowUpDate);

internal sealed class UpdateMedicalRecord : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("medical-record/{medicalRecordId:guid}", async (
            Guid medicalRecordId, 
            UpdateMedicalRecordRequest request, 
            ISender sender) =>
        {
            var command = new UpdateMedicalRecordCommand(
                MedicalRecordId: medicalRecordId,
                Title: request.Title,
                RecommendationForPatient: request.RecommendationForPatient,
                FollowUpDate: request.FollowUpDate);
            
            var result = await sender.Send(command);
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        // TODO.RequireAuthorization(AuthTags.DoctorOrManagerPolicy);
        .WithTags(ApiTags.MedicalRecords)
        .Produces<MedicalRecordResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}