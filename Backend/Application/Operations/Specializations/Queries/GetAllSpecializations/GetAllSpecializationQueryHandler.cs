using Application.Common.Interfaces;
using MediatR;

namespace Application.Operations.Specializations.Queries.GetAllSpecializations;

public class GetAllSpecializationQueryHandler(ISpecializationRepository specializationRepository) 
    : IRequestHandler<GetAllSpecializationQuery, List<SpecializationResponse>>
{
    public async Task<List<SpecializationResponse>> 
        Handle(GetAllSpecializationQuery request, CancellationToken cancellationToken)
    {
        var specializations = await specializationRepository
            .GetSpecializationsAsync(cancellationToken);

        return specializations
            .Select(s => new SpecializationResponse().ToSpecializationResponse(s))
            .ToList();
    }
}