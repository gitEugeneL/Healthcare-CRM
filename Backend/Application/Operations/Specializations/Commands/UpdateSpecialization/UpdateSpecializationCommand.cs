using FluentValidation;
using MediatR;

namespace Application.Operations.Specializations.Commands.UpdateSpecialization;

public sealed record UpdateSpecializationCommand(
    Guid SpecializationId,
    string Description
) : IRequest<SpecializationResponse>;

public sealed class UpdateSpecializationValidator : AbstractValidator<UpdateSpecializationCommand>
{
    public UpdateSpecializationValidator()
    {
        RuleFor(s => s.SpecializationId)
            .NotEmpty();

        RuleFor(s => s.Description)
            .NotEmpty()
            .MaximumLength(200);
    }
}