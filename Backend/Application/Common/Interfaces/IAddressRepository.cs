using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAddressRepository
{
    Task<Address> UpdateAddressAsync(Address address, CancellationToken cancellationToken);

    Task<Address?> FindAddressByIdAsync(Guid id, CancellationToken cancellationToken);
}
