using Api.Utils;
using Application.Common.Exceptions;
using Application.Operations.AppointSettings;
using Application.Operations.AppointSettings.Commands.Config;
using Application.Operations.AppointSettings.Queries.GetAppointmentSettings;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class AppointmentSettingsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/appointment-settings")
            .WithTags("Appointment settings");
        
        group.MapPut("", Config)
            .RequireAuthorization(AuthPolicy.DoctorPolicy)
            .Produces<AppointmentSettingsResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status422UnprocessableEntity);
        
        group.MapGet("{settingsId:guid}", GetOne)
            .RequireAuthorization()
            .Produces<AppointmentSettingsResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }

    private async Task<Results<Ok<AppointmentSettingsResponse>, NotFound<string>, UnprocessableEntity<string>>> Config(
        [FromBody] ConfigAppointmentCommand command,
        HttpContext httpContext,
        ISender sender)
    {
        try
        {
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (UnauthorizedException exception)
        {
            return TypedResults.UnprocessableEntity(exception.Message);
        }
    }

    private async Task<Results<Ok<AppointmentSettingsResponse>, NotFound<string>>> GetOne(
        Guid settingsId,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetConfigAppointmentQuery(settingsId)));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }
}