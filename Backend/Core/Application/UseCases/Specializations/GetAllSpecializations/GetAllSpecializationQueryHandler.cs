using Domain.Abstractions.Errors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.GetAllSpecializations;

internal sealed class GetAllSpecializationQueryHandler(
    ISpecializationRepository specializationRepository
) : IRequestHandler<GetAllSpecializationQuery, Result<IReadOnlyList<SpecializationResponse>>>
    {
    public async Task<Result<IReadOnlyList<SpecializationResponse>>> Handle(
        GetAllSpecializationQuery query, 
        CancellationToken ct)
    {
        var specializations = await specializationRepository.GetAllSpecializationsWithDoctorsAsync(ct);
        
        return specializations
            .Select(SpecializationResponse.FromSpecialization)
            .ToList();
    }
}