using Application.Common.Models;
using Application.Operations.MedicalRecords;
using Application.Operations.MedicalRecords.Commands.CreateMedicalRecord;
using Application.Operations.MedicalRecords.Commands.UpdateMedicalRecord;
using Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForDoctor;
using Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForPatient;
using Application.Operations.MedicalRecords.Queries.GetMedicalRecord;
using Asp.Versioning;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/medical-record")]
public class MedicalRecordController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(Role.Doctor)}")]
    [ProducesResponseType(typeof(MedicalRecordResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<MedicalRecordResponse>> Create([FromBody] CreateMedicalRecordCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Created(result.MedicalRecordId.ToString(), result);
    }

    [HttpPut]
    [Authorize(Roles = $"{nameof(Role.Doctor)}")]
    [ProducesResponseType(typeof(MedicalRecordResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<MedicalRecord>> Update([FromBody] UpdateMedicalRecordCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{medicalRecordId:guid}")]
    [Authorize(Roles = $"{nameof(Role.Doctor)}, {nameof(Role.Patient)}")]
    [ProducesResponseType(typeof(MedicalRecordResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<MedicalRecord>> GetOne(Guid medicalRecordId)
    {
        var id = CurrentUserId();
        var role = CurrentUserRole();
        if (id is null || role is null)
            return BadRequest();

        var query = new GetMedicalRecordQuery(medicalRecordId);
        query.SetCurrentUserId(id);
        query.SerCurrentUserRole(role);

        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("for-patient")]
    [Authorize(Roles = nameof(Role.Patient))]
    [ProducesResponseType(typeof(PaginatedList<MedicalRecordResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<MedicalRecordResponse>>>
        GetAllForPatient([FromQuery] GetAllRecordsForPatientQueryPagination query)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        query.SetCurrentUserId(id);
        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("for-doctor")]
    [Authorize(Roles = nameof(Role.Doctor))]
    [ProducesResponseType(typeof(PaginatedList<MedicalRecordResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<MedicalRecordResponse>>>
        GetAllForDoctor([FromQuery] GetAllRecordsForDoctorQueryPagination query)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        query.SetCurrentUserId(id);
        var result = await Mediator.Send(query);

        return Ok(result);
    }
}
