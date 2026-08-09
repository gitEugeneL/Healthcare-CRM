using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Doctors.UpdateDoctor;

internal sealed class UpdateDoctorValidator : AbstractValidator<UpdateDoctorCommand>
{
    public UpdateDoctorValidator()
    {
        Include(new UpdateUserValidator());
        
        RuleFor(d => d.DoctorId)
            .NotEmpty()
            .WithMessage("The user id is required.");      
        
        RuleFor(d => d.Education)
            .MaximumLength(150);
        
        RuleFor(d => d.Description)
            .MaximumLength(250);  
    }
}