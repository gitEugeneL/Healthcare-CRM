namespace Application.Common.Models;

public sealed record JwtToken(string AccessToken, string Type = "Bearer");
