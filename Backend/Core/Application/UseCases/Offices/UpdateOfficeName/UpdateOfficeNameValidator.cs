using FluentValidation;

namespace Application.UseCases.Offices.UpdateOfficeName;

internal sealed class UpdateOfficeNameValidator : AbstractValidator<UpdateOfficeNameCommand>
{
    public UpdateOfficeNameValidator()
    {
        RuleFor(o => o.OfficeId)
            .NotEmpty()
            .WithMessage("The office id is required.");
        
        RuleFor(o => o.Name)
            .NotEmpty()
            .WithMessage("The name is required.");       
    }
}