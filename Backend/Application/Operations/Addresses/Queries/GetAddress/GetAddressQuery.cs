using MediatR;

namespace Application.Operations.Addresses.Queries.GetAddress;

public sealed record GetAddressQuery(Guid AddressId) : IRequest<AddressResponse>;
