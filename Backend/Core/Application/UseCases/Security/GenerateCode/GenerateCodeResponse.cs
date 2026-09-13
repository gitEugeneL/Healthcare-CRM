namespace Application.UseCases.Security.GenerateCode;

public sealed record GenerateCodeResponse(string Email, DateTime Expires);