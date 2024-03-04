using FluentValidation;
using MediatR;

namespace Application.Operations.Specializations.Commands.ExcludeDoctor;

public sealed record ExcludeDoctorCommand(
    Guid UserDoctorId,
    Guid SpecializationId
) : IRequest<SpecializationResponse>;

public sealed class ExcludeDoctorValidator : AbstractValidator<ExcludeDoctorCommand>
{
    public ExcludeDoctorValidator()
    {
        RuleFor(e => e.UserDoctorId)
            .NotEmpty();

        RuleFor(e => e.SpecializationId)
            .NotEmpty();
    }
}
