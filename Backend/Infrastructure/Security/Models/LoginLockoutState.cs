namespace Security.Models;

public sealed record LoginLockoutState(
    int FailedCount = 0,
    bool Locked = false,
    DateTime? LockExpires = null
);
