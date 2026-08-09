using Application.UseCases.Common.User;
using Domain.Managers;

namespace Application.UseCases.Managers;

public sealed record ManagerResponse(
    Guid ManagerId,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Position
) : UserResponse(Email, FirstName, LastName, Phone)
{
    public static ManagerResponse FromManager(Manager manager)
    {
        return new ManagerResponse(
            ManagerId: manager.Id,
            Email: manager.User.Email, 
            FirstName: manager.User.FirstName, 
            LastName: manager.User.LastName, 
            Phone: manager.User.Phone, 
            Position: manager.Position
        );
    }
}