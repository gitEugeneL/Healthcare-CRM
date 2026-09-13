using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Specializations;
using Application.UseCases.Specializations.ExcludeDoctor;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Specializations;

internal sealed class ExcludeDoctorFromSpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("specialization/{specializationId:guid}/exclude-doctor/{doctorId:guid}", async (
            Guid specializationId,
            Guid doctorId,
            ISender sender) =>
        {
            var command = new ExcludeDoctorCommand(
                SpecializationId: specializationId,
                DoctorId: doctorId);
            
            Result<SpecializationResponse> result = await sender.Send(command);
            return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
        })
        .RequireAuthorization(AuthTags.ManagerPolicy)
        .WithTags(EndpointTags.Specializations)
        .Produces<SpecializationResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status400BadRequest);
    } 
}