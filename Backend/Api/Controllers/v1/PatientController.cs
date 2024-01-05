using Application.Common.Models;
using Application.Operations.Doctor;
using Application.Operations.Patients;
using Application.Operations.Patients.Commands.CreatePatient;
using Application.Operations.Patients.Commands.DeletePatient;
using Application.Operations.Patients.Commands.UpdatePatient;
using Application.Operations.Patients.Queries.GetAllPatients;
using Application.Operations.Patients.Queries.GetPatient;
using Asp.Versioning;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/patient")]
public class PatientController(IMediator mediator) : BaseController(mediator)
{
     [HttpPost]
     [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status201Created)]
     public async Task<ActionResult<PatientResponse>> Create([FromBody] CreatePatientCommand command)
     {
         var result = await Mediator.Send(command);
         return Created(result.UserId.ToString(), result);
     }
     
     [HttpPut]
     [Authorize(Roles = $"{nameof(Role.Patient)}")]
     [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
     public async Task<ActionResult<PatientResponse>> Update([FromBody] UpdatePatientCommand command)
     {
         var id = CurrentUserId();
         if (id is null)
             return BadRequest();

         command.SetCurrentUserId(id);
         var result = await Mediator.Send(command);
         return Ok(result);
     }
     
     [HttpDelete]
     [Authorize(Roles = $"{nameof(Role.Patient)}")]
     [ProducesResponseType(StatusCodes.Status204NoContent)]
     public async Task<ActionResult> Delete()
     {
         var id = CurrentUserId();
         if (id is null)
             return BadRequest();
         
         await Mediator.Send(new DeletePatientCommand().SetCurrentUserId(id));
         return Ok();
     }
     
     [HttpGet("{userId:guid}")]
     [Authorize(Roles = $"{nameof(Role.Doctor)}, {nameof(Role.Manager)}")]
     [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
     public async Task<ActionResult<PatientResponse>> GetOne(Guid userId)
     {
         var result = await Mediator.Send(new GetPatientQuery(userId));
         return Ok(result);
     }
     
     [HttpGet]
     [Authorize(Roles = $"{nameof(Role.Doctor)}, {nameof(Role.Manager)}")]
     [ProducesResponseType(typeof(PaginatedList<DoctorResponse>), StatusCodes.Status200OK)]
     public async Task<ActionResult<PaginatedList<DoctorResponse>>> GetAll([FromQuery] GetAllPatientsQueryPagination q)
     {
         var result = await Mediator.Send(q);
         return Ok(result);
     }
}
