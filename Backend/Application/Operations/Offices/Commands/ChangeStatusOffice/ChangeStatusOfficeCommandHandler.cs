using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Offices.Commands.ChangeStatusOffice;

public class ChangeStatusOfficeCommandHandler(IOfficeRepository officeRepository) 
    : IRequestHandler<ChangeStatusOfficeCommand, OfficeResponse>
{
    public async Task<OfficeResponse> Handle(ChangeStatusOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await officeRepository.FindOfficeByIdAsync(request.OfficeId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Office), request.OfficeId);

        office.IsAvailable = !office.IsAvailable;
        var updatedOffice = await officeRepository.UpdateOfficeAsync(office, cancellationToken);
        return new OfficeResponse().ToOfficeResponse(updatedOffice);
    }
}
