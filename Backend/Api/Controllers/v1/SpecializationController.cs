using Application.Operations.Specializations;
using Application.Operations.Specializations.Commands.CreateSpecialization;
using Application.Operations.Specializations.Commands.DeleteSpecialization;
using Application.Operations.Specializations.Commands.ExcludeDoctor;
using Application.Operations.Specializations.Commands.IncludeDoctor;
using Application.Operations.Specializations.Commands.UpdateSpecialization;
using Application.Operations.Specializations.Queries.GetAllSpecializations;
using Asp.Versioning;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/specialization")]
public class SpecializationController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<SpecializationResponse>> Create([FromBody] CreateSpecializationCommand command)
    {
        var result = await Mediator.Send(command);
        return Created(result.SpecializationId.ToString(), result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SpecializationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SpecializationResponse>>> GetAll([FromQuery] GetAllSpecializationQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SpecializationResponse>> Update([FromBody] UpdateSpecializationCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{specializationId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid specializationId)
    {
        await Mediator.Send(new DeleteSpecializationCommand(specializationId));
        return NoContent();
    }

    [HttpPut("include-doctor")]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SpecializationResponse>> IncludeDoctor([FromBody] IncludeDoctorCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("exclude-doctor")]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SpecializationResponse>> ExcludeDoctor([FromBody] ExcludeDoctorCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
