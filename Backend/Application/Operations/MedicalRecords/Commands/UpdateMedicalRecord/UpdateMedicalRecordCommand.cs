using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.MedicalRecords.Commands.UpdateMedicalRecord;

public sealed record UpdateMedicalRecordCommand : CurrentUser, IRequest<MedicalRecordResponse> 
{
    [Required]
    public Guid MedicalRecordId { get; init; }

    [MaxLength(50)] 
    public string? Tittle { get; init; }

    public string? DoctorNote { get; init; }
}