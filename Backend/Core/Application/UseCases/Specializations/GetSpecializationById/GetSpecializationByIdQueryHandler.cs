using Domain.Abstractions.Errors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.GetSpecializationById;

internal sealed class GetSpecializationByIdQueryHandler(
    ISpecializationRepository specializationRepository
    ) : IRequestHandler<GetSpecializationByIdQuery, Result<SpecializationResponse>>
{
    public async Task<Result<SpecializationResponse>> Handle(GetSpecializationByIdQuery query, CancellationToken ct)
    {
        var result = await specializationRepository
            .FindSpecializationByIdWithDoctorsAndAsync(query.SpecializationId, ct);

        return result is null
            ? Result.Failure<SpecializationResponse>(SpecializationErrors.NotFound(query.SpecializationId))
            : SpecializationResponse.FromSpecialization(result);
    }
}