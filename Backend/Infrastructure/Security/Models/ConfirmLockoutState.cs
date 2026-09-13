namespace Security.Models;

public sealed record ConfirmLockoutState(
    int GenerateCodeCount = 0,
    int FailedCount = 0,
    bool Locked = false,
    DateTime? LockExpires = null
);