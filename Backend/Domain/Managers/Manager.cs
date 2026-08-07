using Domain.Abstractions.Errors;
using Domain.Common;
using Domain.Users;

namespace Domain.Managers;

public sealed class Manager : BaseAuditableEntity
{
    private Manager() {}
    
    public string? Position { get; private set; }

    /*** Relations ***/
    public User User { get; private init; } = null!;
    public Guid UserId { get; private init; }

    public static Result<Manager> Create(string? position, User user)
    {
        if (user.Role != UserAuthRole.Manager)
        {
            return Result.Failure<Manager>(ManagerErrors.InvalidRole);
        }
        
        var manager = new Manager
        {
            Position = position,
            User = user
        };

        return manager;
    }

    public void UpdatePosition(string position)
    {
        if (Position == position)
        {
            return;
        }
        Position = position;
    }
}

