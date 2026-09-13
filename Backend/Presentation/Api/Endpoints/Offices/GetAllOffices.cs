using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Offices;
using Application.UseCases.Offices.GetAllOffices;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Offices;

internal class GetAllOffices : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("offices", async (ISender sender) =>
            {
                var query = new GetAllOfficesQuery();
                Result<IReadOnlyList<OfficeResponse>> result = await sender.Send(query);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.BasePolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Offices)
            .Produces<IReadOnlyList<OfficeResponse>>();
    }
}