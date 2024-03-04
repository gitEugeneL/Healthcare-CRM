using FluentValidation;
using MediatR;

namespace Application.Operations.Offices.Commands.UpdateOffice;

public sealed record UpdateOfficeCommand(
    Guid OfficeId, 
    string Name
) : IRequest<OfficeResponse>;

public sealed class UpdateOfficeValidator : AbstractValidator<UpdateOfficeCommand>
{
    public UpdateOfficeValidator()
    {
        RuleFor(o => o.OfficeId)
            .NotEmpty();

        RuleFor(o => o.Name)
            .NotEmpty();
    }
}
