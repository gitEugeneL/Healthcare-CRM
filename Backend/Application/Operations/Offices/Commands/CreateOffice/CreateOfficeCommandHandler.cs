using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Offices.Commands.CreateOffice;

public class CreateOfficeCommandHandler(IOfficeRepository officeRepository)
    : IRequestHandler<CreateOfficeCommand, OfficeResponse>
{
    public async Task<OfficeResponse> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
    {
        if (await officeRepository.FindOfficeByNumberAsync(request.Number, cancellationToken) is not null)
            throw new AlreadyExistException(nameof(Office), request.Number);

        var office = await officeRepository.CreateOfficeAsync(
            new Office
            {
                Name = request.Name,
                Number = request.Number,
                IsAvailable = true
            },
            cancellationToken
        );

        return new OfficeResponse().ToOfficeResponse(office);
    }
}
