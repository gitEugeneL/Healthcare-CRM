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
                AppointmentSettings = new AppointmentSettings
                {
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(18, 0),
                    Interval = Interval.H1,
                    Workdays = 
                    [
                        Workday.Monday,
                        Workday.Tuesday,
                        Workday.Wednesday,
                        Workday.Thursday,
                        Workday.Friday,
                        Workday.Saturday,
                        Workday.Sunday
                    ]
                },
                User = new User
                {
                    Email = request.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = Role.Doctor
                }
            },
            cancellationToken
        );

        return new DoctorResponse()
            .ToDoctorResponse(doctor);
    }
}
