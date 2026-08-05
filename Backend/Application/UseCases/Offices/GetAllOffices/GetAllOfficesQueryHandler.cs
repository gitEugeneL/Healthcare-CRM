using Domain.Abstractions.Errors;
using Domain.Offices;
using MediatR;

namespace Application.UseCases.Offices.GetAllOffices;

internal sealed class GetAllOfficesQueryHandle(
    IOfficeRepository officeRepository    
) : IRequestHandler<GetAllOfficesQuery, Result<IReadOnlyList<OfficeResponse>>>
{
    public async Task<Result<IReadOnlyList<OfficeResponse>>> Handle(GetAllOfficesQuery query, CancellationToken ct)
    {
        return (await officeRepository.GetAllOfficesAsync(ct))
            .Select(OfficeResponse.FromOffice)
            .ToList();
    }
}