namespace Application.Abstractions.Security.Models;

public sealed record AttemptState(int Count = 0, DateTime? LockedUntil = null)
{
    public bool IsLocked => LockedUntil is not null && LockedUntil > DateTime.UtcNow;
}