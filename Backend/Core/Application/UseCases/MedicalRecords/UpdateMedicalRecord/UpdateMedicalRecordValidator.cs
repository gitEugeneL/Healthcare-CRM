using FluentValidation;

namespace Application.UseCases.MedicalRecords.UpdateMedicalRecord;

internal sealed class UpdateMedicalRecordValidator : AbstractValidator<UpdateMedicalRecordCommand>
{
    public UpdateMedicalRecordValidator()
    {
        RuleFor(mr => mr.MedicalRecordId)
            .NotEmpty()
            .WithMessage("The medical record id is required.");       
        
        RuleFor(mr => mr.Title)
            .MaximumLength(50)
            .WithMessage("The title must be less than 50 characters.");    
        
        RuleFor(mr => mr.RecommendationForPatient)
            .MaximumLength(250)
            .WithMessage("The recommendation for patient must be less than 250 characters.");
        
        RuleFor(mr => mr.FollowUpDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("The follow up date must be in the future.");
    }
}