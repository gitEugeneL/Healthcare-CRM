using Api.Helpers;
using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Operations.Managers;
using Application.Operations.Managers.Commands.CreateManager;
using Application.Operations.Managers.Commands.UpdateManager;
using Carter;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class ManagerEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/manager")
            .WithApiVersionSet(ApiVersioning.VersionSet(app))
            .MapToApiVersion(1)
            .WithTags(nameof(UserManager));
        
        group.MapPost("", Create)
            .RequireAuthorization(AppConstants.AdminPolicy)
            .WithValidator<CreateMangerCommand>()
            .Produces<ManagerResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("", Update)
            .RequireAuthorization(AppConstants.ManagerPolicy)
            .WithValidator<UpdateManagerCommand>()
            .Produces<ManagerResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }

    private async Task<Results<Created<ManagerResponse>, Conflict<string>>> Create(
        [FromBody] CreateMangerCommand command,
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

    private async Task<Results<Ok<ManagerResponse>, NotFound<string>>> Update(
        [FromBody] UpdateManagerCommand command,
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
}