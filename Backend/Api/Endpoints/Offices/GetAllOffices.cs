using Api.ApiResults;
using Application.UseCases.Offices;
using Application.UseCases.Offices.GetAllOffices;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Offices;

internal static class GetAllOffices
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("offices", async (ISender sender) =>
            {
                var query = new GetAllOfficesQuery();
                Result<IReadOnlyList<OfficeResponse>> result = await sender.Send(query);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .WithTags(ApiTags.Offices)
            .Produces<IReadOnlyList<OfficeResponse>>();
    }
}