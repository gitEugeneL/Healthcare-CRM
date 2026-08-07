using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Offices.ChangeOfficeStatus;

public sealed record ChangeOfficeStatusCommand(Guid OfficeId) : IRequest<Result<OfficeResponse>>;