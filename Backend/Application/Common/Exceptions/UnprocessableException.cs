namespace Application.Common.Exceptions;

public sealed class UnprocessableException(string message) : Exception(message);
