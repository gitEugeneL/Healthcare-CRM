using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Specializations;
using Application.UseCases.Specializations.GetSpecializationById;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Specializations;

internal sealed class GetSpecializationById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("specialization/{specializationId:guid}", async (Guid specializationId,  ISender sender) =>
            {
                var query = new GetSpecializationByIdQuery(specializationId);
                
                Result<SpecializationResponse> result = await sender.Send(query); 
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(EndpointTags.Specializations)
            .Produces<SpecializationResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }
}