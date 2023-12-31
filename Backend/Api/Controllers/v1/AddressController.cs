using Application.Operations.Addresses;
using Application.Operations.Addresses.Commands.UpdateAddress;
using Application.Operations.Addresses.Queries.GetAddress;
using Asp.Versioning;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/address")]
public class AddressController(IMediator mediator) : BaseController(mediator)
{
    [HttpPut]
    [Authorize(Roles = $"{nameof(Role.Patient)}")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressResponse>> Update([FromBody] UpdateAddressCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{addressId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressResponse>> GetOne(Guid addressId)
    {
        var result = await Mediator.Send(new GetAddressQuery(addressId));
        return Ok(result);
    }
}
