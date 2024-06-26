using Api.Helpers;
using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Operations.Specializations;
using Application.Operations.Specializations.Commands.CreateSpecialization;
using Application.Operations.Specializations.Commands.DeleteSpecialization;
using Application.Operations.Specializations.Commands.ExcludeDoctor;
using Application.Operations.Specializations.Commands.IncludeDoctor;
using Application.Operations.Specializations.Commands.UpdateSpecialization;
using Application.Operations.Specializations.Queries.GetAllSpecializations;
using Carter;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class SpecializationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/specialization")
            .WithApiVersionSet(ApiVersioning.VersionSet(app))
            .MapToApiVersion(1)
            .WithTags(nameof(Specialization));

        group.MapPost("", Create)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .WithValidator<CreateSpecializationCommand>()
            .Produces<SpecializationResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("", Update)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .WithValidator<UpdateSpecializationCommand>()
            .Produces<SpecializationResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("", GetAll)
            .Produces<List<SpecializationResponse>>();

        group.MapDelete("{specializationId:guid}", Delete)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("include-doctor", IncludeDoctor)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .WithValidator<IncludeDoctorCommand>()
            .Produces<SpecializationEndpoints>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("exclude-doctor", ExcludeDoctor)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .WithValidator<ExcludeDoctorCommand>()
            .Produces<SpecializationResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }

    private async Task<Results<Created<SpecializationResponse>, Conflict<string>>> Create(
        [FromBody] CreateSpecializationCommand command,
        ISender sender)
    {
        try
        {
            var result = await sender.Send(command);
            return TypedResults.Created(result.SpecializationId.ToString(), result);
        }
        catch (AlreadyExistException exception)
        {
            return TypedResults.Conflict(exception.Message);
        }
    }

    private async Task<Results<Ok<SpecializationResponse>, NotFound<string>>> Update(
        [FromBody] UpdateSpecializationCommand command,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<IResult> GetAll(
        [AsParameters] GetAllSpecializationQuery query,
        ISender sender)
    {
        return TypedResults.Ok(await sender.Send(query));
    }

    private async Task<Results<NoContent, NotFound<string>>> Delete(
        Guid specializationId,
        ISender sender)
    {
        try
        {
            await sender.Send(new DeleteSpecializationCommand(specializationId));
            return TypedResults.NoContent();
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<Results<Ok<SpecializationResponse>, NotFound<string>, Conflict<string>>> IncludeDoctor(
        [FromBody] IncludeDoctorCommand command,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (AlreadyExistException exception)
        {
            return TypedResults.Conflict(exception.Message);
        }
    }

    private async Task<Results<Ok<SpecializationResponse>, NotFound<string>>> ExcludeDoctor(
        [FromBody] ExcludeDoctorCommand command,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }    
    }
}