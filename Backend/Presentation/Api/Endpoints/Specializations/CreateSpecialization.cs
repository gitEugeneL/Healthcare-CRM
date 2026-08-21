using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Specializations;
using Application.UseCases.Specializations.CreateSpecialization;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Specializations;

internal sealed record CreateSpecializationRequest(
    string Name, 
    string? Description);

internal sealed class CreateSpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("specializations", async (CreateSpecializationRequest request, ISender sender) =>
            {
                var command = new CreateSpecializationCommand(
                    Name: request.Name,
                    Description: request.Description);

                Result<SpecializationResponse> result = await sender.Send(command);
           
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            //TODO .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(ApiTags.Specializations)
            .Produces<SpecializationResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
    }
}