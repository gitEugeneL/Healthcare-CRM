using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Specializations.Commands.CreateSpecialization;

public class CreateSpecializationCommandHandler(ISpecializationRepository specializationRepository) 
    : IRequestHandler<CreateSpecializationCommand, SpecializationResponse>
{
    public async Task<SpecializationResponse> 
        Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        if (await specializationRepository.FindSpecializationByValueAsync(request.Value, cancellationToken) is not null)
            throw new AlreadyExistException(nameof(Specialization), request.Value);

        var specialization = await specializationRepository.CreateSpecializationAsync(
            new Specialization
            {
                Value = request.Value,
                Description = request.Description
            },
            cancellationToken
        );

        return new SpecializationResponse()
            .ToSpecializationResponse(specialization);
    }
}
