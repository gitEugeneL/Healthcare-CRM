using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Doctors;
using Application.UseCases.Doctors.GetDoctor;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Doctors;

internal class GetDoctorByUserId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("doctors/{doctorId:guid}", async (Guid doctorId, ISender sender) =>
            {
                var query = new GetDoctorQuery(doctorId);
                Result<DoctorResponse> result = await sender.Send(query);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(ApiTags.Doctors)
            .Produces<DoctorResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }
}