using MediatR;

namespace Application.Operations.Offices.Queries.GetAllOffices;

public sealed record GetAllOfficesQuery : IRequest<List<OfficeResponse>>;
