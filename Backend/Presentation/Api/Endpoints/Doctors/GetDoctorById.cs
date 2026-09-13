using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Doctors;
using Application.UseCases.Doctors.GetDoctor;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Doctors;

internal class GetDoctorById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("doctors/{doctorId:guid}", async (Guid doctorId, ISender sender) =>
            {
                var query = new GetDoctorQuery(doctorId);
                Result<DoctorResponse> result = await sender.Send(query);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.ManagerOrPatientPolicy)
            .WithTags(EndpointTags.Doctors)
            .Produces<DoctorResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }
}