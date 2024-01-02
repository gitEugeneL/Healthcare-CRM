using Application.Operations.Appointment;
using Application.Operations.Appointment.Commands.CreateAppointment;
using Application.Operations.Appointment.Queries.FindFreeHours;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/appointment")]
public class AppointmentController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("find-time")]
    [Authorize(Roles = $"{nameof(Role.Patient)}")]
    [ProducesResponseType(typeof(FreeHoursResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FreeHoursResponse>> FindFreeHours([FromBody] FindFreeHoursQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    // [HttpPost]
    // [Authorize(Roles = $"{nameof(Role.Patient)}")]
    // [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    // public async Task<ActionResult<AppointmentResponse>> Create([FromBody] CreateAppointmentCommand command)
    // {
    //     var result = await Mediator.Send(command);
    //     return Created("------------------", result);
    // }
    
    
    // todo showForManager
    // todo showForDoctor
    // todo showForPatient
    // todo finalize
    // todo cancel
}
