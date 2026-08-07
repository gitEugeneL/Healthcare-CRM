using Application.UseCases.Common.User;
using FluentValidation;

namespace Application.UseCases.Managers.UpdateManager;

internal sealed class UpdateManagerValidator : AbstractValidator<UpdateManagerCommand>
{
    public UpdateManagerValidator()
    {
        Include(new UpdateUserValidator());
        
        RuleFor(m => m.Position)
            .MaximumLength(100)
            .WithMessage("The position must be less than 100 characters.");
    }
}