using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Doctor.Commands.CreateDoctor;

public class CreateDoctorCommandHandler(
    IUserRepository userRepository,
    IDoctorRepository doctorRepository,
    IPasswordManager passwordManager
    )
    : IRequestHandler<CreateDoctorCommand, DoctorResponse>
{
    public async Task<DoctorResponse> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.FindUserByEmailAsync(request.Email, cancellationToken) is not null)
            throw new AlreadyExistException(nameof(User), request.Email);
        
        passwordManager.CreatePasswordHash(request.Password, out var hash, out var salt);

        var doctor = await doctorRepository.CreateDoctorAsync(
            new UserDoctor
            {
                Status = Status.Active,
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

        return new DoctorResponse()
            .ToDoctorResponse(doctor);
    }
}