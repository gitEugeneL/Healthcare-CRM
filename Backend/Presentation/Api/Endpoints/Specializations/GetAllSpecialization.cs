using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Specializations;
using Application.UseCases.Specializations.GetAllSpecializations;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Specializations;

internal sealed class GetAllSpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Specialization", async (ISender sender) =>
            {
                var query = new GetAllSpecializationQuery();
                Result<IReadOnlyList<SpecializationResponse>> result = await sender.Send(query);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .AllowAnonymous()
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Specializations)
            .Produces<IReadOnlyList<SpecializationResponse>>();
    }
}