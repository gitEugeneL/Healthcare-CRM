using Application.Common.Models;
using Application.Operations.Doctor;
using Application.Operations.Doctor.Commands.CreateDoctor;
using Application.Operations.Doctor.Commands.UpdateDoctor;
using Application.Operations.Doctor.Queries.GetAllDoctors;
using Application.Operations.Doctor.Queries.GetDoctor;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/doctor")]
public class DoctorController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [Authorize(Roles = nameof(Role.Manager))]
    [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<DoctorResponse>> Create([FromBody] CreateDoctorCommand command)
    {
        var result = await Mediator.Send(command);
        return Created(result.UserId.ToString(), result);
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.Doctor))]
    [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DoctorResponse>> Update([FromBody] UpdateDoctorCommand command)
    {
        var userId = CurrentUserId();
        if (userId is null)
            return BadRequest();
        
        command.SetCurrentUserId(userId);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DoctorResponse>> GetOne(Guid userId)
    {
        var result = await Mediator.Send(new GetDoctorQuery(userId));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<DoctorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<DoctorResponse>>> GetAll([FromQuery] GetAllDoctorsQueryPagination query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
