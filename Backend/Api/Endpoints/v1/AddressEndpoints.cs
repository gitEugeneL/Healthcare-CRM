using Api.Helpers;
using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Operations.Addresses;
using Application.Operations.Addresses.Commands.UpdateAddress;
using Application.Operations.Addresses.Queries.GetAddress;
using Carter;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class AddressEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/address")
            .WithApiVersionSet(ApiVersioning.VersionSet(app))
            .MapToApiVersion(1)
            .WithTags(nameof(Address));
        
        group.MapPut("", Update)
            .WithValidator<UpdateAddressCommand>()
            .RequireAuthorization(AppConstants.PatientPolicy)
            .Produces<AddressResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{addressId:guid}", GetOne)
            .RequireAuthorization(AppConstants.BasePolicy)
            .Produces<AddressResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }
    
    private async Task<Results<Ok<AddressResponse>, NotFound<string>>> Update(
        [FromBody] UpdateAddressCommand command,
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
    }

    private async Task<Results<Ok<AddressResponse>, NotFound<string>>> GetOne(
        Guid addressId,
        ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetAddressQuery(addressId)));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }
}