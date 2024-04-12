using Api.Helpers;
using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Operations.MedicalRecords;
using Application.Operations.MedicalRecords.Commands.CreateMedicalRecord;
using Application.Operations.MedicalRecords.Commands.UpdateMedicalRecord;
using Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForDoctor;
using Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForPatient;
using Application.Operations.MedicalRecords.Queries.GetMedicalRecord;
using Carter;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class MedicalRecordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/medical-record")
            .WithApiVersionSet(ApiVersioning.VersionSet(app))
            .MapToApiVersion(1)
            .WithTags(nameof(MedicalRecord));
        
        group.MapPost("", Create)
            .RequireAuthorization(AppConstants.DoctorPolicy)
            .WithValidator<CreateMedicalRecordCommand>()
            .Produces<MedicalRecordResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("", Update)
            .RequireAuthorization(AppConstants.DoctorPolicy)
            .WithValidator<UpdateMedicalRecordCommand>()
            .Produces<MedicalRecordResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{medicalRecordId:guid}", GetOne)
            .RequireAuthorization(AppConstants.DoctorOrPatientPolicy)
            .Produces<MedicalRecordResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet("for-patient", GetAllForPatient)
            .RequireAuthorization(AppConstants.PatientPolicy)
            .Produces<PaginatedList<MedicalRecordResponse>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("for-doctor", GetAllForDoctor)
            .RequireAuthorization(AppConstants.DoctorPolicy)
            .Produces<PaginatedList<MedicalRecordResponse>>()
            .Produces(StatusCodes.Status404NotFound);
    }

    private async Task<Results<Created<MedicalRecordResponse>, UnauthorizedHttpResult, Conflict<string>, NotFound<string>>> 
        Create(
            [FromBody] CreateMedicalRecordCommand command, 
            ISender sender, 
            HttpContext httpContext)
    {
        try
        {
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            var result = await sender.Send(command);
            return TypedResults.Created(result.MedicalRecordId.ToString(), result);
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (UnauthorizedException exception)
        {
            return TypedResults.Unauthorized();
        }
        catch (AlreadyExistException exception)
        {
            return TypedResults.Conflict(exception.Message);
        }
    }

    private async Task<Results<Ok<MedicalRecordResponse>, UnauthorizedHttpResult, NotFound<string>>> Update(
        [FromBody] UpdateMedicalRecordCommand command,
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            command.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (UnauthorizedException exception)
        {
            return TypedResults.Unauthorized();
        }
    }

    private async Task<Results<Ok<MedicalRecordResponse>, ForbidHttpResult, NotFound<string>>> GetOne(
        Guid medicalRecordId,
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            var query = new GetMedicalRecordQuery(medicalRecordId);
            query.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            query.SerCurrentUserRole(BaseService.ReadUserRoleFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(query));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
        catch (AccessDeniedException exception)
        {
            return TypedResults.Forbid();
        }
    }

    private async Task<Results<Ok<PaginatedList<MedicalRecordResponse>>, NotFound<string>>> GetAllForPatient(
        [AsParameters] GetAllRecordsForPatientQueryPagination query,
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            query.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(query));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }

    private async Task<Results<Ok<PaginatedList<MedicalRecordResponse>>, NotFound<string>>> GetAllForDoctor(
        [AsParameters] GetAllRecordsForDoctorQueryPagination query,
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            query.SetCurrentUserId(BaseService.ReadUserIdFromToken(httpContext));
            return TypedResults.Ok(await sender.Send(query));
        }
        catch (NotFoundException exception)
        {
            return TypedResults.NotFound(exception.Message);
        }
    }
}