using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Domain.RefreshTokens;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Security;

public sealed class TokenManager : ITokenManager
{
    private readonly SymmetricSecurityKey _signingKey;
    private readonly int _tokenLifetimeMin;
    private readonly int _refreshTokenLifetimeDays;

    public TokenManager(IConfiguration configuration)
    {
        var key = configuration["Authentication:Key"]
            ?? throw new InvalidOperationException("Authentication:Key is not configured.");

        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        _tokenLifetimeMin = int.Parse(
            configuration["Authentication:TokenLifetimeMin"]
            ?? throw new InvalidOperationException("Authentication:TokenLifetimeMin is not configured."));

        _refreshTokenLifetimeDays = int.Parse(
            configuration["Authentication:RefreshTokenLifetimeDays"]
            ?? throw new InvalidOperationException("Authentication:RefreshTokenLifetimeDays is not configured."));
    }

    public string GenerateAccessToken(User user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        ];

        var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha512Signature);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenLifetimeMin),
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        var refreshToken = RefreshToken.Create(
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(256)),
            DateTime.UtcNow.AddDays(_refreshTokenLifetimeDays),
            user);

        return refreshToken.Value;
    }
}