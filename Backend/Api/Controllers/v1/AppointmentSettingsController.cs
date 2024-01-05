using Application.Operations.AppointSettings;
using Application.Operations.AppointSettings.Commands.Config;
using Application.Operations.AppointSettings.Queries.GetAppointmentSettings;
using Asp.Versioning;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/appointment-settings")]
public class AppointmentSettingsController(IMediator mediator) : BaseController(mediator)
{
    [HttpPut]
    [Authorize(Roles = $"{nameof(Role.Doctor)}")]
    [ProducesResponseType(typeof(AppointmentSettingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AppointmentSettingsResponse>> Config([FromBody] ConfigAppointmentCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("{settingsId:guid}")]
    [ProducesResponseType(typeof(AppointmentSettingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AppointmentSettingsResponse>> GetOne(Guid settingsId)
    {
        var result = await Mediator.Send(new GetConfigAppointmentQuery(settingsId));
        return Ok(result);
    }
}
