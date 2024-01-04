using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.MedicalRecords.Commands.CreateMedicalRecord;

public sealed record CreateMedicalRecordCommand : CurrentUser, IRequest<MedicalRecordResponse>
{
    [Required] 
    public Guid UserPatientId { get; init; }

    [Required]
    public Guid AppointmentId { get; init; }

    [Required]
    [MaxLength(50)]
    public required string Title { get; init; }

    [Required]
    public required string DoctorNote { get; init; }
}
