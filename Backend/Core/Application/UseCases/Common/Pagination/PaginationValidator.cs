using FluentValidation;

namespace Application.UseCases.Common.Pagination;

internal sealed class PaginationValidator : AbstractValidator<PaginationQuery>
{
    public PaginationValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");
        
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(10)
            .WithMessage("Page size must be less than or equal to 10");       
    }
}