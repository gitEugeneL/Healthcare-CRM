using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Operations.Offices;
using Application.Operations.Offices.Commands.ChangeStatusOffice;
using Application.Operations.Offices.Commands.CreateOffice;
using Application.Operations.Offices.Commands.UpdateOffice;
using Application.Operations.Offices.Queries.GetAllOffices;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class OfficeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/office")
            .WithTags("Office");

        group.MapPost("", Create)
            .RequireAuthorization(AuthPolicy.ManagerPolicy)
            .WithValidator<CreateOfficeCommand>()
            .Produces<OfficeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("", Update)
            .RequireAuthorization(AuthPolicy.ManagerPolicy)
            .WithValidator<UpdateOfficeCommand>()
            .Produces<OfficeResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPatch("", ChangeStatus)
            .RequireAuthorization(AuthPolicy.DoctorOrManagerPolicy)
            .Produces<OfficeResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("", GetAll)
            .RequireAuthorization(AuthPolicy.DoctorOrManagerPolicy)
            .Produces<List<OfficeResponse>>();
    }

    private async Task<Results<Created<OfficeResponse>, Conflict<string>>> Create(
        [FromBody] CreateOfficeCommand command,
        ISender sender)
    {
        try
        {
            var result = await sender.Send(command);
            return TypedResults.Created(result.OfficeId.ToString(), result);
        }
        catch (AlreadyExistException exception)
        {
            return TypedResults.Conflict(exception.Message);
        }
    }

    private async Task<Results<Ok<OfficeResponse>, NotFound<string>>> Update(
        [FromBody] UpdateOfficeCommand command,
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

    private async Task<Results<Ok<OfficeResponse>, NotFound<string>>> ChangeStatus(
        [FromBody] ChangeStatusOfficeCommand command,
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

    private async Task<IResult> GetAll(ISender sender)
    {
        return TypedResults.Ok(await sender.Send(new GetAllOfficesQuery()));
    }
}