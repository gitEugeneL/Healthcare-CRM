using FluentValidation;

namespace Application.UseCases.Offices.ChangeOfficeStatus;

internal sealed class ChangeOfficeStatusValidator : AbstractValidator<ChangeOfficeStatusCommand>
{
    public ChangeOfficeStatusValidator()
    {
        RuleFor(o => o.OfficeId)
            .NotEmpty()
            .WithMessage("The office id is required.");       
    }
}
