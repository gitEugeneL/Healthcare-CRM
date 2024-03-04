using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Operations.MedicalRecords.Commands.UpdateMedicalRecord;

public sealed record UpdateMedicalRecordCommand(
    Guid MedicalRecordId,
    string? Title,
    string? DoctorNote
) : CurrentUser, IRequest<MedicalRecordResponse>; 

public sealed class UpdateMedicalRecordValidator : AbstractValidator<UpdateMedicalRecordCommand>
{
    public UpdateMedicalRecordValidator()
    {
        RuleFor(mr => mr.MedicalRecordId)
            .NotEmpty();

        RuleFor(mr => mr.Title)
            .MaximumLength(50);
    }
}