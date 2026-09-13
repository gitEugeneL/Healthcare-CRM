using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Security;
using Domain.Abstractions.Errors;
using Domain.RefreshTokens;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Security.Services;

internal sealed class TokenService(IConfiguration configuration): ITokenService
{
    private readonly string _settings = configuration["Authentication:AccessToken:SecurityKey"] ?? 
                                       throw new ApplicationException("SecurityKey not found n configuration");

    private readonly DateTime _expires = DateTime.UtcNow.AddMinutes(
        int.Parse(configuration["Authentication:AccessToken:LifetimeInMinutes"] ??
                  throw new ApplicationException("Lifetime not found in config")));
    
    private readonly string _issuer = configuration["Authentication:Issuer"] ?? 
                                      throw new ApplicationException("Issuer not found in config");
    
    private readonly string _audience = configuration["Authentication:Audience"] ??
                                          throw new ApplicationException("Audience not found in config");
    
    private readonly int _lifetimeDays = int.Parse(configuration["Authentication:RefreshToken:LifetimeInDays"] ??
                                                    throw new ApplicationException("RefreshTokenLifetime not found in config"));
    
    private readonly int _maxRefreshTokenCount = int.Parse(configuration["Authentication:RefreshToken:MaxCount"] ?? 
                                                          throw new ApplicationException("MaxRefreshTokenCount not found in config"));
    
    public (string token, DateTime expires) GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _expires,
            SigningCredentials = credentials,
            Issuer = _issuer,
            Audience = _audience
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(handler.CreateToken(descriptor));
    
        return (token, _expires);
    }
    
    public RefreshToken GenerateRefreshToken(User user)
    {
        Result<RefreshToken> result = RefreshToken.Create(
            token: Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            expires: DateTime.UtcNow.AddDays(_lifetimeDays),
            userId: user.Id);

        return result.IsFailure 
            ? throw new ApplicationException("Invalid refreshToken configuration") 
            : result.Value;
    }
    
    public void AddRefreshTokenToUser(User user, RefreshToken refreshToken)
    {
        user.UpdateRefreshTokens(
            refreshToken: refreshToken,
            maxActiveTokens: _maxRefreshTokenCount);
    }

    public void RemoveRefreshTokenFromUser(User user, RefreshToken refreshToken)
    {
        user.RemoveRefreshToken(refreshToken);
    }
}