using FluentValidation;

namespace Application.UseCases.Doctors.ChangeDoctorStatus;

internal sealed class ChangeDoctorStatusValidator : AbstractValidator<ChangeDoctorStatusCommand>
{
    public ChangeDoctorStatusValidator()
    {
        RuleFor(d => d.DoctorId)
            .NotEmpty()
            .WithMessage("The user id is required.");      
    }
}