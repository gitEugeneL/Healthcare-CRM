using Api.Utils;
using Application.Common.Exceptions;
using Application.Operations.Appointments;
using Application.Operations.Appointments.Commands.CancelAppointment;
using Application.Operations.Appointments.Commands.CreateAppointment;
using Application.Operations.Appointments.Commands.FinaliseAppointment;
using Application.Operations.Appointments.Queries.FindFreeHours;
using Application.Operations.Appointments.Queries.GetAllByDate;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class AppointmentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/appointment")
            .WithTags("Appointment");
        
        group.MapPost("", Create)
            .RequireAuthorization(AuthPolicy.PatientPolicy)
            .Produces<AppointmentResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("find-time/{userDoctorId:guid}/{date}", FindFreeHours)
            .RequireAuthorization(AuthPolicy.PatientPolicy)
            .Produces<FreeHoursResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{date}", GetAllByDate)
            .RequireAuthorization()
            .Produces<List<AppointmentResponse>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("finalise/{appointmentId:guid}", Finalise)
            .RequireAuthorization(AuthPolicy.DoctorPolicy)
            .Produces<AppointmentResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity);

        group.MapPut("cancel/{appointmentId:guid}", Cancel)
            .RequireAuthorization(AuthPolicy.DoctorPolicy)
            .Produces<AppointmentResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private async Task<Results<Created<AppointmentResponse>, NotFound<string>>> Create(
        [FromBody] CreateAppointmentCommand command,
        HttpContext httpContext,
        ISender sender)
    {
        try
        {
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            var result = await sender.Send(command);
            return TypedResults.Created(result.AppointmentId.ToString(), result);
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<Results<Ok<FreeHoursResponse>, NotFound<string>>> FindFreeHours(
        Guid userDoctorId,
        string date,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new FindFreeHoursQuery(userDoctorId, date)));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<Results<Ok<List<AppointmentResponse>>, NotFound<string>>> GetAllByDate(
        string date,
        HttpContext httpContext,
        ISender sender)
    {
        try
        {
            var query = new GetAllByDateQuery(date);
            query.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            query.SerCurrentUserRole(BaseService.ReadUserRoleFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(query));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<Results<Ok<AppointmentResponse>, NotFound<string>, BadRequest<string>, UnprocessableEntity<string>>> 
        Finalise(
            Guid appointmentId, 
            HttpContext httpContext, 
            ISender sender)
    {
        try
        {
            var command = new FinaliseAppointmentCommand(appointmentId);
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (AccessDeniedException exception)
        {
            return TypedResults.BadRequest(exception.Message);
        }
        catch (UnprocessableException exception)
        {
            return TypedResults.UnprocessableEntity(exception.Message);
        }
    }

    private async Task<Results<Ok<AppointmentResponse>, NotFound<string>, BadRequest<string>>> Cancel(
        Guid appointmentId, 
        HttpContext httpContext, 
        ISender sender)
    {
        try
        {
            var command = new CancelAppointmentCommand(appointmentId);
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (AccessDeniedException exception)
        {
            return TypedResults.BadRequest(exception.Message);
        }
    }
}