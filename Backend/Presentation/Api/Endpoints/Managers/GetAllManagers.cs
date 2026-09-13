using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Managers;
using Application.UseCases.Managers.GetAllManagers;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

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
            .RequireAuthorization(AuthTags.AdminPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Managers)
            .Produces<IReadOnlyList<ManagerResponse>>();
    }
}