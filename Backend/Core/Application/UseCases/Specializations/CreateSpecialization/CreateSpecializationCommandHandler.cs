using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Specializations;
using MediatR;

namespace Application.UseCases.Specializations.CreateSpecialization;

internal sealed class CreateSpecializationCommandHandler(
    ISpecializationRepository specializationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSpecializationCommand, Result<SpecializationResponse>>
{
    public async Task<Result<SpecializationResponse>> Handle(CreateSpecializationCommand command, CancellationToken ct)
    {
        if (await specializationRepository.SpecializationExistsByNameAsync(command.Name, ct))
        {
            return Result.Failure<SpecializationResponse>(SpecializationErrors.AlreadyExist(command.Name));
        }
        
        Specialization specialization = Specialization.Create(command.Name, command.Description); 
        
        await specializationRepository.InsertSpecializationAsync(specialization, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return SpecializationResponse.FromSpecialization(specialization);
    }
}
