using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUserRepository userRepository,
    IPasswordManager passwordManager
    )
    : IRequestHandler<CreatePatientCommand, PatientResponse>
{
    public async Task<PatientResponse> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.FindUserByEmailAsync(request.Email, cancellationToken) is not null)
            throw new AlreadyExistException(nameof(User), request.Email);

        passwordManager.CreatePasswordHash(request.Password, out var hash, out var salt);

        var patient = await patientRepository.CreatePatientAsync(
            new UserPatient
            {
                User = new User
                {
                    Email = request.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = Role.Patient
                },
                Address = new Address()
            },
            cancellationToken
        );

        return new PatientResponse()
            .ToPatientResponse(patient);
    }
}