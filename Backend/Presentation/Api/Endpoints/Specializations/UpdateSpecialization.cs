using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Specializations;
using Application.UseCases.Specializations.UpdateSpecialization;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Specializations;

internal sealed record UpdateSpecializationRequest(string Description);


public class UpdateSpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("specialization/{specializationId:guid}", async (
                Guid specializationId,
                UpdateSpecializationRequest request,
                ISender sender) =>
            {
                var command = new UpdateSpecializationCommand(
                    SpecializationId: specializationId,
                    Description: request.Description);

                Result<SpecializationResponse> result = await sender.Send(command);

                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            //TODO .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(ApiTags.Specializations)
            .Produces<SpecializationResponse>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}