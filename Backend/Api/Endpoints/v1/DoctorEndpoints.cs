using Api.Helpers;
using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Operations.Doctor;
using Application.Operations.Doctor.Commands.CreateDoctor;
using Application.Operations.Doctor.Commands.UpdateDoctor;
using Application.Operations.Doctor.Queries.GetAllDoctors;
using Application.Operations.Doctor.Queries.GetDoctor;
using Carter;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class DoctorEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/doctor")
            .WithApiVersionSet(ApiVersioning.VersionSet(app))
            .MapToApiVersion(1)
            .WithTags(nameof(UserDoctor));
        
        group.MapPost("", Create)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .WithValidator<CreateDoctorCommand>()
            .Produces<DoctorResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("", Update)
            .RequireAuthorization(AppConstants.DoctorPolicy)
            .WithValidator<UpdateDoctorCommand>()
            .Produces<DoctorResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{userId:guid}", GetOne)
            .Produces<DoctorResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("", GetAll)
            .Produces<PaginatedList<DoctorResponse>>();
    }

    private async Task<Results<Created<DoctorResponse>, Conflict<string>>> Create(
        [FromBody] CreateDoctorCommand command,
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

    private async Task<Results<Ok<DoctorResponse>, NotFound<string>>> Update(
        [FromBody] UpdateDoctorCommand command,
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

    private async Task<Results<Ok<DoctorResponse>, NotFound<string>>> GetOne(
        Guid userId,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetDoctorQuery(userId)));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<IResult> GetAll(
        [AsParameters] GetAllDoctorsQueryPagination query,
        ISender sender)
    {
        return TypedResults.Ok(await sender.Send(query));
    }
}