using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Managers;
using Application.UseCases.Managers.GetAllManagers;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Managers;

internal class GetAllManagers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("managers", async (ISender sender) =>
            {
                var query = new GetAllManagersQuery();
                Result<IReadOnlyList<ManagerResponse>> result = await sender.Send(query);

                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.AdminPolicy)
            .WithTags(ApiTags.Managers)
            .Produces<IReadOnlyList<ManagerResponse>>();
    }
}