using Application.Operations.Managers;
using Application.Operations.Managers.Commands.CreateManager;
using Application.Operations.Managers.Commands.UpdateManager;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/manager")]
public class ManagerController(IMediator mediator) : BaseController(mediator)
{
    // add authorize role (admin)
    [HttpPost]
    [ProducesResponseType(typeof(ManagerResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ManagerResponse>> Create([FromBody] CreateMangerCommand command)
    {
        var result = await Mediator.Send(command);
        return Created(result.UserId.ToString(), result);
    }

    // add authorize role (manager)
    [HttpPut]
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