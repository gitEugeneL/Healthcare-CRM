using System.Security.Claims;

namespace Api.Utils;

internal static class TokenManager
{
    internal static Guid ReadUserIdFromToken(HttpContext httpContext)
    {
        return Guid.Parse(
            httpContext
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    
    internal  static string ReadUserRoleFromToken(HttpContext httpContext)
    {
        return httpContext
            .User
            .FindFirstValue(ClaimTypes.Role)!;
    }
}