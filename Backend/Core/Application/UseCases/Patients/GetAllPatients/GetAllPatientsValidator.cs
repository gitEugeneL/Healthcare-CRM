using Application.UseCases.Common.Pagination;
using FluentValidation;

namespace Application.UseCases.Patients.GetAllPatients;

public class GetAllPatientsValidator : AbstractValidator<GetAllPatientsQuery>
{
    public GetAllPatientsValidator()
    {
        Include(new PaginationValidator());
    }
}