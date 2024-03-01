using System.Security.Claims;

namespace Api.Utils;

public static class BaseService
{
    public static Guid ReadUserIdFromToken(HttpContext httpContext)
    {
        return Guid.Parse(
            httpContext
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    
    public static string ReadUserRoleFromToken(HttpContext httpContext)
    {
        return httpContext
            .User
            .FindFirstValue(ClaimTypes.Role)!;
    }
}