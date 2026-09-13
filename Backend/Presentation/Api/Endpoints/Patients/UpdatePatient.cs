using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Patients;
using Application.UseCases.Patients.UpdatePatient;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Patients;

internal sealed record UpdatePatientRequest(
    string? Phone,
    string? Pesel,
    DateOnly? DateOfBirth,
    string? FirstName,
    string? LastName,
    string? Insurance,
    string? Province,
    string? PostalCode,
    string? City,
    string? Street,
    string? Hose,
    string? Apartment
);

internal sealed class UpdatePatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("patients/{patientId:guid}", async (
            Guid patientId,
            UpdatePatientRequest request,
            ISender sender) =>
            {
                var command = new UpdatePatientCommand(
                    PatientId: patientId,
                    Phone: request.Phone,
                    Pesel: request.Pesel,
                    DateOfBirth: request.DateOfBirth,
                    FirstName: request.FirstName,
                    LastName: request.LastName,
                    Insurance: request.Insurance,
                    Address: new UpdateAddress(
                        Province: request.Province,
                        PostalCode: request.PostalCode,
                        City: request.City,
                        Street: request.Street,
                        Hose: request.Hose,
                        Apartment: request.Apartment));
                
                Result<PatientResponse> result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Patients)
            .Produces<PatientResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}