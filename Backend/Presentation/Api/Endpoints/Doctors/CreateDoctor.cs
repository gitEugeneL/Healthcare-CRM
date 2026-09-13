using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Doctors;
using Application.UseCases.Doctors.CreateDoctor;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Doctors;

internal sealed record CreateDoctorRequest(
    string Email,
    string Password,
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Education,
    string? Description);

internal class CreateDoctor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("doctors", async (CreateDoctorRequest request, ISender sender) =>
        {
            var command = new CreateDoctorCommand(
                Email: request.Email,
                Password: request.Password,
                Phone: request.Phone,
                FirstName: request.FirstName,
                LastName: request.LastName,
                Education: request.Education,
                Description: request.Description);
            
            Result<DoctorResponse> result = await sender.Send(command);
            
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
            .RequireAuthorization(AuthTags.ManagerPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Doctors)
            .Produces<DoctorResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
    }
}