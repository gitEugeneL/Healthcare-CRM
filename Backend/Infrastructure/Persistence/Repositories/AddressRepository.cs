// using Application.Common.Interfaces;
// using Domain.Entities;
// using Persistence.Persistence;
// using Microsoft.EntityFrameworkCore;
//
// namespace Persistence.Repositories;
//
// internal class AddressRepository(DataContext dataContext) : IAddressRepository
// {
//     public async Task<Address> UpdateAddressAsync(Address address, CancellationToken cancellationToken)
//     {
//         dataContext.Addresses.Update(address);
//         await dataContext.SaveChangesAsync(cancellationToken);
//         return address;
//     }
//
//     public async Task<Address?> FindAddressByIdAsync(Guid id, CancellationToken cancellationToken)
//     {
//         return await dataContext.Addresses
//             .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
//     }
// }
