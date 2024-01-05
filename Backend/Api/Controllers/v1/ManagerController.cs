using Application.Operations.Managers;
using Application.Operations.Managers.Commands.CreateManager;
using Application.Operations.Managers.Commands.UpdateManager;
using Asp.Versioning;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/manager")]
public class ManagerController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    [ProducesResponseType(typeof(ManagerResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ManagerResponse>> Create([FromBody] CreateMangerCommand command)
    {
        var result = await Mediator.Send(command);
        return Created(result.UserId.ToString(), result);
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(ManagerResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ManagerResponse>> Update([FromBody] UpdateManagerCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
