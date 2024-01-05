using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Specializations.Commands.DeleteSpecialization;

public class DeleteSpecializationCommandHandler(ISpecializationRepository specializationRepository) 
    : IRequestHandler<DeleteSpecializationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await specializationRepository.FindSpecializationByIdAsync(request.Id, cancellationToken)
                             ?? throw new NotFoundException(nameof(Specialization), request.Id);

        await specializationRepository.DeleteSpecializationAsync(specialization, cancellationToken);
        return await Unit.Task;
    }
}
