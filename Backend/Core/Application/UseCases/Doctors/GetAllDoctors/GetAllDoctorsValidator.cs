using Application.UseCases.Common.Pagination;
using FluentValidation;

namespace Application.UseCases.Doctors.GetAllDoctors;

internal sealed class GetAllDoctorsValidator : AbstractValidator<GetAllDoctorsQuery>
{
    public GetAllDoctorsValidator()
    {
        Include(new PaginationValidator());
    }
}