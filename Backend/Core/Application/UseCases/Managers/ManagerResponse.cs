using Application.UseCases.Common.User;
using Domain.Managers;

namespace Application.UseCases.Managers;

public sealed record ManagerResponse(
    Guid UserId,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Position
) : UserResponse(UserId, Email, FirstName, LastName, Phone)
{
    public static ManagerResponse FromManager(Manager manager)
    {
        return new ManagerResponse(
            manager.UserId, 
            manager.User.Email, 
            manager.User.FirstName, 
            manager.User.LastName, 
            manager.User.Phone, 
            manager.Position
        );
    }
}