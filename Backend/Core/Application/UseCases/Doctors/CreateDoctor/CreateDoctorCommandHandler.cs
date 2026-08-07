using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Doctors.CreateDoctor;

public class CreateDoctorCommandHandler(
   IUserRepository userRepository,
   IDoctorRepository doctorRepository,
   IUnitOfWork unitOfWork,
   IPasswordManager passwordManager) : IRequestHandler<CreateDoctorCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDoctorCommand command, CancellationToken ct)
    {
        if (await userRepository.UserExistsByEmailAsync(command.Email, ct))
        {
            return Result.Failure<Guid>(UserErrors.AlreadyExist(command.Email));
        }
        
        passwordManager.CreatePasswordHash(command.Password, out var hash, out var salt);
        
        Doctor doctor = Doctor.Create(
            education: command.Education,
            description: command.Description,
            user: User.Create(
                email: command.Email,
                passwordHash: hash,
                passwordSalt: salt,
                role: UserAuthRole.Doctor,
                phone: command.Phone,
                firstName: command.FirstName,
                lastName: command.LastName));

        await doctorRepository.InsertDoctorAsync(doctor, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return doctor.Id;
    }
}
