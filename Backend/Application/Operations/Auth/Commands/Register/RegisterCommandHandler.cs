using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository, 
    IPasswordManager passwordManager)
    : IRequestHandler<RegisterCommand, Guid>
{
    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.FindUserByEmailAsync(request.Email, cancellationToken) is not null)
            throw new AlreadyExistException(nameof(User), request.Email);

        passwordManager.CreatePasswordHash(request.Password, out var hash, out var salt);

        var user = await userRepository.CreateUserAsync(
            new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = Role.Admin
            },
            cancellationToken
        );

        return user.Id;
    }
}
