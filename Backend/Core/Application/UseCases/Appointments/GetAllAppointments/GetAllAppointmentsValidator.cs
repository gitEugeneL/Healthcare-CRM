using Application.UseCases.Common.Pagination;
using FluentValidation;

namespace Application.UseCases.Appointments.GetAllAppointments;

internal sealed class GetAllAppointmentsValidator : AbstractValidator<GetAllAppointmentsQuery>
{
    public GetAllAppointmentsValidator()
    {
        Include(new PaginationValidator());
        
        RuleFor(a => a.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddMonths(3)))
            .WithMessage("The date must not be more than three months in the future.");
    }
}