using Domain.Abstractions.Errors;
using Domain.Managers;
using MediatR;

namespace Application.UseCases.Managers.GetAllManagers;

internal sealed class GetAllManagersQueryHandler(
    IManagerRepository managerRepository    
) : IRequestHandler<GetAllManagersQuery, Result<IReadOnlyList<ManagerResponse>>>
{
    public async Task<Result<IReadOnlyList<ManagerResponse>>> Handle(GetAllManagersQuery query, CancellationToken ct)
    {
        return (await managerRepository.GetAllManagersAsync(ct))
            .Select(ManagerResponse.FromManager)
            .ToList();
    }
}