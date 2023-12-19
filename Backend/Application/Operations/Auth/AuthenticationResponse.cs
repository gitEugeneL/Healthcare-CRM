using Application.Common.Models;

namespace Application.Operations.Auth;

public record AuthenticationResponse(
    JwtToken JwtToken,
    CookieToken CookieToken
);