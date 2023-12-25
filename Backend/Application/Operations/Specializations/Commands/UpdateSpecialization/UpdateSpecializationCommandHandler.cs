using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Specializations.Commands.UpdateSpecialization;

public class UpdateSpecializationCommandHandler(ISpecializationRepository specializationRepository) 
    : IRequestHandler<UpdateSpecializationCommand, SpecializationResponse>
{
    public async Task<SpecializationResponse> 
        Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await specializationRepository
                                 .FindSpecializationByIdAsync(request.SpecializationId, cancellationToken)
                             ?? throw new NotFoundException(nameof(Specialization), request.SpecializationId);

        specialization.Description = request.Description;

        var updateSpecialization = await specializationRepository
            .UpdateSpecializationAsync(specialization, cancellationToken);

        return new SpecializationResponse()
            .ToSpecializationResponse(updateSpecialization);
    }
}
