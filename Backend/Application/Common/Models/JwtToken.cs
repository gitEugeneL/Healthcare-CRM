namespace Application.Common.Models;

public record JwtToken(string AccessToken, string Type = "Bearer");
