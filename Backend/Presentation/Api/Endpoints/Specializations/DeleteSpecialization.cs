using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Specializations.DeleteSpecialization;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Specializations;

internal sealed class DeleteSpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("specialization/{specializationId:guid}", async (Guid specializationId, ISender sender) =>
            {
                var command = new DeleteSpecializationCommand(specializationId);
                
                Result<Unit> result = await sender.Send(command);
                return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
            })
        //TODO .RequireAuthorization(AuthTags.ManagerPolicy)
        .WithTags(ApiTags.Specializations)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);
    }
}