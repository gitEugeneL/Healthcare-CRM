using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Managers.Commands.CreateManager;

public class CreateManagerHandler(
    IUserRepository userRepository, 
    IManagerRepository managerRepository, 
    IPasswordManager passwordManager
    ) 
    : IRequestHandler<CreateMangerCommand, ManagerResponse>
{
    public async Task<ManagerResponse> Handle(CreateMangerCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.FindUserByEmailAsync(request.Email, cancellationToken) is not null)
            throw new AlreadyExistException(nameof(User), request.Email);

        passwordManager.CreatePasswordHash(request.Password, out var hash, out var salt);

        var manager = await managerRepository.CreateManagerAsync(
            new UserManager
            {
                Position = "start position",
                User = new User
                {
                    Email = request.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = Role.Manager
                }
            }, 
            cancellationToken
        );
        
        return new ManagerResponse()
            .ToManagerResponse(manager);
    }
}
