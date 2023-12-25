using MediatR;

namespace Application.Operations.Specializations.Queries.GetAllSpecializations;

public sealed record GetAllSpecializationQuery : IRequest<List<SpecializationResponse>>;
