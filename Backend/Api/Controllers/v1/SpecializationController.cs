using Application.Operations.Specializations;
using Application.Operations.Specializations.Commands.CreateSpecialization;
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
    
    // todo get list

    // todo update
    
    // todo delete
    
    // todo include doctor
    
    // todo exclude doctor
}
