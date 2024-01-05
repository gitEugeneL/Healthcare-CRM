using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Offices.Commands.UpdateOffice;

public class UpdateOfficeCommandHandler(IOfficeRepository officeRepository)
    : IRequestHandler<UpdateOfficeCommand, OfficeResponse>
{
    public async Task<OfficeResponse> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await officeRepository.FindOfficeByIdAsync(request.OfficeId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Office), request.OfficeId);

        office.Name = request.Name;
        var updatedOffice = await officeRepository.UpdateOfficeAsync(office, cancellationToken);
        return new OfficeResponse().ToOfficeResponse(updatedOffice);
    }
}
