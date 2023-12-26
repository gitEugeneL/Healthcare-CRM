using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(
    IPatientRepository patientRepository,
    IAddressRepository addressRepository
    ) 
    : IRequestHandler<UpdateAddressCommand, AddressResponse>
{
    public async Task<AddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.FindPatientByUserIdAsync(request.CurrentUserId, cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
        
        var address = patient.Address;
        address.Province = request.Province ?? address.Province;
        address.PostalCode = request.PostalCode ?? address.PostalCode;
        address.City = request.City ?? address.City;
        address.Street = request.Street ?? address.Street;
        address.Hose = request.Hose ?? address.Hose;
        address.Apartment = request.Apartment ?? address.Apartment;

        var updatedAddress = await addressRepository.UpdateAddressAsync(address, cancellationToken);
        
        return new AddressResponse()
            .ToAddressResponse(updatedAddress);
    }
}