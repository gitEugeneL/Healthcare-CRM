using MediatR;

namespace Application.Operations.Auth.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<AuthenticationResponse>;
