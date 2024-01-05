using Application.Common.Interfaces;
using MediatR;

namespace Application.Operations.Offices.Queries.GetAllOffices;

public class GetAllOfficesQueryHandler(IOfficeRepository officeRepository) 
    : IRequestHandler<GetAllOfficesQuery, List<OfficeResponse>>
{
    public async Task<List<OfficeResponse>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
    {
        var offices = await officeRepository.FindOfficesAsync(cancellationToken);
        return offices
            .Select(office => new OfficeResponse().ToOfficeResponse(office))
            .ToList();
    }
}
