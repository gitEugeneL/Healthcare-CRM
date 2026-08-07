namespace Application.Common.Exceptions;

public sealed class AlreadyExistException(string name, object key) 
    : Exception($"Entity: {name} ({key}) already exists");