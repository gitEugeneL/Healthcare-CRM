using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Offices.UpdateOfficeName;

public sealed record UpdateOfficeNameCommand(
    Guid OfficeId,
    string Name
) : IRequest<Result<OfficeResponse>>;