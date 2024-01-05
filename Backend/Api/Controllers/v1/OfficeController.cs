using Application.Operations.Offices;
using Application.Operations.Offices.Commands.ChangeStatusOffice;
using Application.Operations.Offices.Commands.CreateOffice;
using Application.Operations.Offices.Commands.UpdateOffice;
using Application.Operations.Offices.Queries.GetAllOffices;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/office")]
public class OfficeController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(OfficeResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OfficeResponse>> Create([FromBody] CreateOfficeCommand command)
    {
        var result = await Mediator.Send(command);
        return Created(result.OfficeId.ToString(), result);
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(OfficeResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OfficeResponse>> Update([FromBody] UpdateOfficeCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(Roles = $"{nameof(Role.Doctor)}, {nameof(Role.Manager)}")]
    [ProducesResponseType(typeof(OfficeResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OfficeResponse>> ChangeStatus([FromBody] ChangeStatusOfficeCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(Role.Doctor)}, {nameof(Role.Manager)}")]
    [ProducesResponseType(typeof(List<OfficeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OfficeResponse>>> GetAll()
    {
        var result = await Mediator.Send(new GetAllOfficesQuery());
        return Ok(result);
    }
}
