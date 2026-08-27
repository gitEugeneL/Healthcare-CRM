using Application.UseCases.Common.Pagination;
using Application.UseCases.MedicalRecords.GetAllMedicalRecords;
using FluentValidation;

namespace Application.UseCases.MedicalRecords.CreateMedicalRecord;

public class GetAllMedicalRecordsValidator : AbstractValidator<GetAllMedicalRecordsQuery>
{
    public GetAllMedicalRecordsValidator()
    {
        Include(new PaginationValidator());
    }
}