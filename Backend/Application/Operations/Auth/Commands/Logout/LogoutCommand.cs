using MediatR;

namespace Application.Operations.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Unit>;
