using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.UpdateSpecialization;

internal sealed class UpdateSpecializationCommandHandler(
    ISpecializationRepository specializationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSpecializationCommand, Result<SpecializationResponse>>
{
    public async Task<Result<SpecializationResponse>> Handle(UpdateSpecializationCommand command, CancellationToken ct)
    {
        var specialization = await specializationRepository
            .FindSpecializationByIdWithTrackingAsync(command.SpecializationId, ct);

        if (specialization is null)
        {
            return Result.Failure<SpecializationResponse>(SpecializationErrors.NotFound(command.SpecializationId));
        }
        
        specialization.UpdateDescription(command.Description);
        await unitOfWork.SaveChangesAsync(ct);

        return SpecializationResponse.FromSpecialization(specialization);
    }
}
