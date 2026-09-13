using Microsoft.AspNetCore.Http;

namespace Security.Utils;

public static class CookieManager
{
    public const string RefreshToken = "refreshToken";
    
    public static void SetCookie(string cookieName, HttpContext context, string value, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires,
        };
        context.Response.Cookies.Append(cookieName, value, cookieOptions);
    }
    
    public static string? ReadCookie(string cookieName, HttpContext context)
    {
        context.Request.Cookies.TryGetValue(cookieName, out var refreshToken);
        return refreshToken;
    }
    
    public static void RemoveCookie(string cookieName, HttpContext context)
    {
        context.Response.Cookies.Delete(cookieName);
    }
}
