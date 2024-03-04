using FluentValidation;
using MediatR;

namespace Application.Operations.Specializations.Commands.IncludeDoctor;

public sealed record IncludeDoctorCommand(
    Guid UserDoctorId,
    Guid SpecializationId
) : IRequest<SpecializationResponse>;

public sealed class IncludeDoctorValidator : AbstractValidator<IncludeDoctorCommand>
{
    public IncludeDoctorValidator()
    {
        RuleFor(e => e.UserDoctorId)
            .NotEmpty();

        RuleFor(e => e.SpecializationId)
            .NotEmpty();
    }
}
