using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.MedicalRecords;
using Application.UseCases.MedicalRecords.CreateMedicalRecord;
using MediatR;

namespace Api.Endpoints.MedicalRecords;

internal sealed record CreateMedicalRecordRequest(
    Guid AppointmentId,
    string Title,
    string DoctorNote,
    string RecommendationForPatient,
    string Diagnosis,
    string IcdCode,
    DateOnly? FollowUpDate);

internal sealed class CreateMedicalRecord : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("medical-record", async (
            CreateMedicalRecordRequest request,
            ISender sender) => 
        {
            var command = new CreateMedicalRecordCommand(
                AppointmentId: request.AppointmentId,
                Title: request.Title,
                DoctorNote: request.DoctorNote,
                RecommendationForPatient: request.RecommendationForPatient,
                Diagnosis: request.Diagnosis,
                IcdCode: request.IcdCode,
                FollowUpDate: request.FollowUpDate);
            
            var result = await sender.Send(command);
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        // TODO .RequireAuthorization(AuthTags.DoctorPolicy)
        .WithTags(ApiTags.MedicalRecords)
        .Produces<MedicalRecordResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);
    }
}