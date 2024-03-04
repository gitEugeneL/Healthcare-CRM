using Application.Operations.Users.Commands;
using FluentValidation;
using MediatR;

namespace Application.Operations.Doctor.Commands.CreateDoctor;

public sealed record CreateDoctorCommand : CreateUserCommand, IRequest<DoctorResponse>;

public sealed class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
            .WithMessage("The password must contain at least one letter, one special character, and one digit.");
    }
}
