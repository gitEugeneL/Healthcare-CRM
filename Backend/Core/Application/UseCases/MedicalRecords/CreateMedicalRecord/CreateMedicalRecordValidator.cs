using FluentValidation;

namespace Application.UseCases.MedicalRecords.CreateMedicalRecord;

internal sealed class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordCommand>
{
    public CreateMedicalRecordValidator()
    {
        RuleFor(mr => mr.AppointmentId)
            .NotEmpty()
            .WithMessage("The appointment id is required.");     
        
        RuleFor(mr => mr.Title)
            .NotEmpty()
            .WithMessage("The title is required.")
            .MaximumLength(50)
            .WithMessage("The title must be less than 50 characters.");      
        
        RuleFor(mr => mr.DoctorNote)
            .NotEmpty()
            .WithMessage("The doctor note is required.")
            .MaximumLength(250)
            .WithMessage("The doctor note must be less than 250 characters.");     
        
        RuleFor(mr => mr.RecommendationForPatient)
            .NotEmpty()
            .WithMessage("The recommendation for patient is required.")
            .MaximumLength(250)
            .WithMessage("The recommendation for patient must be less than 250 characters.");
        
        RuleFor(mr => mr.Diagnosis)
            .NotEmpty()
            .WithMessage("The diagnosis is required.")
            .MaximumLength(150)
            .WithMessage("The diagnosis must be less than 150 characters.");      
        
        RuleFor(mr => mr.IcdCode)
            .NotEmpty()
            .WithMessage("The ICD code is required.")
            .MaximumLength(20)
            .WithMessage("The ICD code must be less than 20 characters.");      
        
        RuleFor(mr => mr.FollowUpDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("The follow up date must be in the future.");
    }
}