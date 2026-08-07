using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Managers.GetAllManagers;

public sealed record GetAllManagersQuery : IRequest<Result<IReadOnlyList<ManagerResponse>>>;