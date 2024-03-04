using FluentValidation;
using MediatR;

namespace Application.Operations.Offices.Commands.CreateOffice;

public record CreateOfficeCommand(
    string Name,
    int Number
) : IRequest<OfficeResponse>;

public sealed class CreateOfficeValidator : AbstractValidator<CreateOfficeCommand>
{
    public CreateOfficeValidator()
    {
        RuleFor(o => o.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(o => o.Number)
            .NotEmpty()
            .InclusiveBetween(1, 9999);
    }
}