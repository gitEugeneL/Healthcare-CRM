using MediatR;

namespace Application.Operations.Offices.Commands.ChangeStatusOffice;

public sealed record ChangeStatusOfficeCommand(Guid OfficeId) : IRequest<OfficeResponse>;
