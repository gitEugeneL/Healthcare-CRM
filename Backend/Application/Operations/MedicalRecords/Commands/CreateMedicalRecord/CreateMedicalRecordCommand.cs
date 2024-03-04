using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Operations.MedicalRecords.Commands.CreateMedicalRecord;

public sealed record CreateMedicalRecordCommand(
    Guid UserPatientId,
    Guid AppointmentId,
    string Title,
    string DoctorNote
) : CurrentUser, IRequest<MedicalRecordResponse>;

public sealed class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordCommand>
{
    public CreateMedicalRecordValidator()
    {
        RuleFor(mr => mr.UserPatientId)
            .NotEmpty();
        
        RuleFor(mr => mr.AppointmentId)
            .NotEmpty();
        
        RuleFor(mr => mr.Title)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(mr => mr.DoctorNote)
            .NotEmpty();
    }
}

