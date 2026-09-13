using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Doctors;
using Application.UseCases.Doctors.UpdateDoctor;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Doctors;

internal sealed record UpdateDoctorRequest(
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Description,
    string? Education
);

internal sealed class UpdateDoctor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("doctors/{doctorId:guid}", async (
            Guid doctorId, 
            UpdateDoctorRequest request,
            ISender sender) =>
        {
            var command = new UpdateDoctorCommand(
                DoctorId: doctorId, 
                Phone: request.Phone,
                FirstName: request.FirstName, 
                LastName: request.LastName, 
                Description: request.Description,
                Education: request.Education 
                );
            
                Result<DoctorResponse> result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
                
        })
        .RequireAuthorization(AuthTags.ManagerPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
        .WithTags(EndpointTags.Doctors)
        .Produces<DoctorResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}