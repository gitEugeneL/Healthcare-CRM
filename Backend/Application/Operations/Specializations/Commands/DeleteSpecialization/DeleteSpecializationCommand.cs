using MediatR;

namespace Application.Operations.Specializations.Commands.DeleteSpecialization;

public sealed record DeleteSpecializationCommand(Guid Id) : IRequest<Unit>;
