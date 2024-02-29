using Api.Utils;
using Application.Common.Exceptions;
using Application.Operations.Addresses;
using Application.Operations.Addresses.Commands.UpdateAddress;
using Application.Operations.Addresses.Queries.GetAddress;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class AddressEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/address")
            .WithTags("Address");

        group.MapPut("", Update)
            .RequireAuthorization(AuthPolicy.PatientPolicy)
            .Produces<AddressResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{addressId:guid}", GetOne)
            .RequireAuthorization()
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