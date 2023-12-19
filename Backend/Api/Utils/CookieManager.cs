namespace Api.Utils;

public static class CookieManager
{
    public static void SetCookie(HttpResponse response, string cookieName, string value, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = expires,
        };
        response.Cookies.Append(cookieName, value, cookieOptions);
    }

    public static void RemoveCookie(HttpResponse response, string cookieName)
    {
        response.Cookies.Delete(cookieName);
    }
}
