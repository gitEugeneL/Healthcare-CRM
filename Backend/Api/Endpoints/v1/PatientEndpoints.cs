using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Operations.Patients;
using Application.Operations.Patients.Commands.CreatePatient;
using Application.Operations.Patients.Commands.DeletePatient;
using Application.Operations.Patients.Commands.UpdatePatient;
using Application.Operations.Patients.Queries.GetAllPatients;
using Application.Operations.Patients.Queries.GetPatient;
using Application.Operations.Users.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class PatientEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/patient")
            .WithTags("Patient");

        group.MapPost("", Create)
            .WithValidator<CreatePatientCommand>()
            .WithValidator<CreatePatientCommand>()
            .Produces<PatientResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("", Update)
            .RequireAuthorization(AuthPolicy.PatientPolicy)
            .WithValidator<UpdatePatientCommand>()
            .Produces<PatientResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("", Delete)
            .RequireAuthorization(AuthPolicy.PatientPolicy)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{userId:guid}", GetOne)
            .RequireAuthorization(AuthPolicy.DoctorOrManagerPolicy)
            .Produces<PatientResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("", GetAll)
            .RequireAuthorization(AuthPolicy.DoctorOrManagerPolicy)
            .Produces<PaginatedList<PatientResponse>>();
    }

    private async Task<Results<Created<PatientResponse>, Conflict<string>, ValidationProblem>> Create(
        [FromBody] CreatePatientCommand command,
        ISender sender)
    {
        try
        {
            var result = await sender.Send(command);
            return TypedResults.Created(result.UserId.ToString(), result);
        }
        catch (AlreadyExistException exception)
        {
            return TypedResults.Conflict(exception.Message);
        }
    }

    private async Task<Results<Ok<PatientResponse>, NotFound<string>>> Update(
        [FromBody] UpdatePatientCommand command,
        ISender sender,
        HttpContext httpContext)
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
    }

    private async Task<Results<NoContent, NotFound<string>>> Delete(
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            var command = new DeletePatientCommand();
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            await sender.Send(command);
            return TypedResults.NoContent();
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<Results<Ok<PatientResponse>, NotFound<string>>> GetOne(
        Guid userId,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetPatientQuery(userId)));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<IResult> GetAll(
        [AsParameters] GetAllPatientsQueryPagination query,
        ISender sender)
    {
        return TypedResults.Ok(await sender.Send(query));
    }
}