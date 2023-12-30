namespace Application.Common.Exceptions;

public sealed class TimeMismatchException(string message) : Exception(message);
