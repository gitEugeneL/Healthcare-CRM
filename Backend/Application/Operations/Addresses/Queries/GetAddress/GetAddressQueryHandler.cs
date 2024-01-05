using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Addresses.Queries.GetAddress;

public class GetAddressQueryHandler(IAddressRepository addressRepository)
    : IRequestHandler<GetAddressQuery, AddressResponse>
{
    public async Task<AddressResponse> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var address = await addressRepository.FindAddressByIdAsync(request.AddressId, cancellationToken)
                      ?? throw new NotFoundException(nameof(Address), request.AddressId);

        return new AddressResponse()
            .ToAddressResponse(address);
    }
}
