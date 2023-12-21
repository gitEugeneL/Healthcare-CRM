using Application.Operations.Common.Users;
using Domain.Entities;

namespace Application.Operations.Managers;

public sealed record ManagerResponse : UserResponse
{
    public string? Position { get; set; }

    public ManagerResponse ToManagerResponse(UserManager manager)
    {
        UserId = manager.UserId;
        Email = manager.User.Email;
        FirstName = manager.User.FirstName;
        LastName = manager.User.LastName;
        Phone = manager.User.Phone;
        Position = manager.Position;
        
        return this;
    }
}
