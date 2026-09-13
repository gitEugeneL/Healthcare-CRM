using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Patients;
using Application.UseCases.Patients.CreatePatient;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Patients;

internal sealed record CreatePatientRequest(
    string Email,
    string Password,
    string Phone,
    string Pesel,
    DateOnly DateOfBirth,
    string? FirstName,
    string? LastName,
    string? Insurance,
    string Province,
    string PostalCode,
    string City,
    string Street,
    string Hose,
    string? Apartment
);

internal sealed class CreatePatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("patients", async (CreatePatientRequest request, ISender sender) =>
            {
                var command = new CreatePatientCommand(
                    Email: request.Email,
                    Password: request.Password,
                    Phone: request.Phone,
                    Pesel: request.Pesel,
                    DateOfBirth: request.DateOfBirth,
                    FirstName: request.FirstName,
                    LastName: request.LastName,
                    Insurance: request.Insurance,
                    Address: new PatientAddress(
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
            .Produces(StatusCodes.Status409Conflict);
    }
}