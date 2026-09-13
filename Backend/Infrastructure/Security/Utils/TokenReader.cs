using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Security.Utils;

public static class TokenReader
{
    public static Guid ReadUserIdFromToken(HttpContext httpContext)
    {
        return Guid.Parse(
            httpContext
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public static string ReadUserEmailFromToken(HttpContext httpContext)
    {
        return httpContext
            .User
            .FindFirstValue(ClaimTypes.Email)!;
    }
    
    public static string ReadUserRoleFromToken(HttpContext httpContext)
    {
        return httpContext
            .User
            .FindFirstValue(ClaimTypes.Role)!;
    }
}