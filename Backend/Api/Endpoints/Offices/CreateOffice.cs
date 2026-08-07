using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Offices.CreateOffice;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Offices;

internal sealed record CreateOfficeRequest(
    string Name, 
    int Number
);

internal class CreateOffice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("offices", async (CreateOfficeRequest request, ISender sender) =>
            {
                var command = new CreateOfficeCommand(request.Name, request.Number);
                Result<Guid> result = await sender.Send(command);

                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(ApiTags.Offices)
            .Produces<Guid>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
    }
}
