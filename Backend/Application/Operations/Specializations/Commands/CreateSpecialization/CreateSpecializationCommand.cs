using FluentValidation;
using MediatR;

namespace Application.Operations.Specializations.Commands.CreateSpecialization;

public sealed record CreateSpecializationCommand(
    string Value,
    string? Description
) : IRequest<SpecializationResponse>;

public sealed class CreateSpecializationValidator : AbstractValidator<CreateSpecializationCommand>
{
    public CreateSpecializationValidator()
    {
        RuleFor(s => s.Value)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(s => s.Description)
            .NotEmpty()
            .MaximumLength(200);
    }
}