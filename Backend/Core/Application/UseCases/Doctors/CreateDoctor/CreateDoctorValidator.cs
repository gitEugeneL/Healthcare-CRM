using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Doctors.CreateDoctor;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
    {
        Include(new CreateUserValidator());
        
        RuleFor(d => d.Education)
            .MaximumLength(150)
            .WithMessage("The education must be less than 150 characters.");       

        RuleFor(d => d.Description)
            .MaximumLength(250)
            .WithMessage("The description must be less than 250 characters.");
    }
}