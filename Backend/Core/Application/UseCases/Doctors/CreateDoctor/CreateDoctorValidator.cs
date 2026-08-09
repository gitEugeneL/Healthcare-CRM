using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Doctors.CreateDoctor;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
    {
        Include(new CreateUserValidator());
        
        RuleFor(d => d.Education)
            .MaximumLength(150);

        RuleFor(d => d.Description)
            .MaximumLength(250);
    }
}