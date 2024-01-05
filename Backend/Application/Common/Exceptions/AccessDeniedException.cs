namespace Application.Common.Exceptions;

public sealed class AccessDeniedException(string name, object key)
    : Exception($"Entity: {name} ({key}) doesn't exist or your password is incorrect");
