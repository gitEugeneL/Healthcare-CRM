using Application.Operations.Specializations;
using Application.Operations.Specializations.Commands.CreateSpecialization;
using Application.Operations.Specializations.Commands.DeleteSpecialization;
using Application.Operations.Specializations.Commands.ExcludeDoctor;
using Application.Operations.Specializations.Commands.IncludeDoctor;
using Application.Operations.Specializations.Commands.UpdateSpecialization;
using Application.Operations.Specializations.Queries.GetAllSpecializations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/specialization")]
public class SpecializationController(IMediator mediator) : BaseController(mediator)
{
    // add authorize role (manager)
    [HttpPost]
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

    // add authorize role (manager)
    [HttpPut]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SpecializationResponse>> Update([FromBody] UpdateSpecializationCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    // add authorize role (manager)
    [HttpDelete("{specializationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid specializationId)
    {
        await Mediator.Send(new DeleteSpecializationCommand(specializationId));
        return NoContent();
    }

    // add authorize role (manager)
    [HttpPut("include-doctor")]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SpecializationResponse>> IncludeDoctor([FromBody] IncludeDoctorCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    // add authorize role (manager)
    [HttpPut("exclude-doctor")]
    [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SpecializationResponse>> ExcludeDoctor([FromBody] ExcludeDoctorCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
