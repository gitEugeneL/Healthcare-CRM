using Domain.RefreshTokens;

namespace Application.UseCases.Security;

public sealed record LoginOrRefreshResponse(
    string AccessToken,
    DateTime AccessTokenExpires,
    RefreshToken RefreshToken)
{
    public static LoginOrRefreshResponse FromTokens(
        string accessToken, 
        DateTime accessTokenExpires, 
        RefreshToken refreshToken)
    {
        return new LoginOrRefreshResponse(accessToken, accessTokenExpires, refreshToken);
    }
}
