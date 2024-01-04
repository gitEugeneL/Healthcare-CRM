using Application.Operations.Appointments;
using Application.Operations.Appointments.Commands.CancelAppointment;
using Application.Operations.Appointments.Commands.CreateAppointment;
using Application.Operations.Appointments.Commands.FinaliseAppointment;
using Application.Operations.Appointments.Queries.FindFreeHours;
using Application.Operations.Appointments.Queries.GetAllByDate;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/appointment")]
public class AppointmentController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(Role.Patient)}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AppointmentResponse>> Create([FromBody] CreateAppointmentCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Created(result.AppointmentId.ToString(), result);
    }

    [HttpGet("find-time/{userDoctorId:guid}/{date}")]
    [Authorize(Roles = $"{nameof(Role.Patient)}")]
    [ProducesResponseType(typeof(FreeHoursResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FreeHoursResponse>> FindFreeHours(Guid userDoctorId, string date)
    {
        var query = new FindFreeHoursQuery(userDoctorId, date);
        if (!TryValidateModel(query))
            return BadRequest(ModelState);
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{date}")]
    [ProducesResponseType(typeof(List<AppointmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AppointmentResponse>>> GetAllByDate(string date)
    {
        var id = CurrentUserId();
        var role = CurrentUserRole();
        if (id is null || role is null)
            return BadRequest();
        
        var query = new GetAllByDateQuery(date);
        query.SetCurrentUserId(id);
        query.SerCurrentUserRole(role);

        if (!TryValidateModel(query))
            return BadRequest(ModelState);

        var result = await Mediator.Send(query); 
        return Ok(result);
    }

    [Authorize(Roles = $"{nameof(Role.Doctor)}")]
    [HttpPut("finalise/{appointmentId:guid}")]
    [ProducesResponseType(typeof(List<AppointmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<AppointmentResponse>> Finalise(Guid appointmentId)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        var command = new FinaliseAppointmentCommand(appointmentId);
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [Authorize(Roles = $"{nameof(Role.Doctor)}")]
    [HttpPut("cancel/{appointmentId:guid}")]
    [ProducesResponseType(typeof(List<AppointmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<AppointmentResponse>> Cancel(Guid appointmentId)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        var command = new CancelAppointmentCommand(appointmentId);
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
    
    // todo cancel
}
