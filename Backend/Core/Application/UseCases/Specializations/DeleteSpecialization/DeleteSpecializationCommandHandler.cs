using Domain.Abstractions.Errors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.DeleteSpecialization;

internal sealed class DeleteSpecializationCommandHandler(
    ISpecializationRepository specializationRepository
) : IRequestHandler<DeleteSpecializationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteSpecializationCommand command, CancellationToken ct)
    {

        var result = await specializationRepository.IsSpecializationEmptyByIdAsync(command.SpecializationId, ct);
        if (result is null)
            return Result.Failure<Unit>(SpecializationErrors.NotFound(command.SpecializationId));

        if (result is false)
            return Result.Failure<Unit>(SpecializationErrors.NotEmpty(command.SpecializationId));

        await specializationRepository.DeleteSpecializationByIdAsync(command.SpecializationId, ct);
        return Unit.Value;
    }
}
