using Application.Abstractions.Security;
using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Addresses;
using Domain.Patients;
using Domain.Users;
using MediatR;

namespace Application.UseCases.Patients.CreatePatient;

internal sealed class CreatePatientCommandHandler(
    IUserRepository userRepository,
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork,
    IPasswordService passwordService) : IRequestHandler<CreatePatientCommand, Result<PatientResponse>>
{
    public async Task<Result<PatientResponse>> Handle(CreatePatientCommand command, CancellationToken ct)
    {
        if (await userRepository.UserExistsByEmailAsync(command.Email, ct))
        {
            return Result.Failure<PatientResponse>(UserErrors.AlreadyExist(command.Email));
        }
        
        passwordService.CreatePasswordHash(command.Password, out var hash, out var salt);

        Patient patient = Patient.Create(
            dateOfBirth: command.DateOfBirth,
            pesel: command.Pesel,
            insurance: command.Insurance,
            user: User.Create(
                email: command.Email,
                passwordHash: hash,
                passwordSalt: salt,
                role: UserAuthRole.Patient,
                phone: command.Phone,
                firstName: command.FirstName,
                lastName: command.LastName),
            address: Address.Create(
                province: command.Address.Province,
                postalCode: command.Address.PostalCode, 
                city: command.Address.City,
                street: command.Address.Street,
                hose: command.Address.Hose,
                apartment: command.Address.Apartment)
        );
        
        await patientRepository.InsertPatientAsync(patient, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return PatientResponse.FromPatient(patient);
    }
}