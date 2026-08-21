using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Specializations;
using Application.UseCases.Specializations.IncludeDoctor;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Specializations;

internal sealed class IncludeDoctorToSpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("specialization/{specializationId:guid}/include-doctor/{doctorId:guid}", async (
                Guid specializationId,
                Guid doctorId,
                ISender sender) =>
            {
                var command = new IncludeDoctorCommand(
                    SpecializationId: specializationId,
                    DoctorId: doctorId);

                Result<SpecializationResponse> result = await sender.Send(command);
                return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
            })
            //TODO .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(ApiTags.Specializations)
            .Produces<SpecializationResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest);
    }
}