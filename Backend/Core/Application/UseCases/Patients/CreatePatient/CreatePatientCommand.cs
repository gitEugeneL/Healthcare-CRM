using Application.UseCases.Common.User;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Patients.CreatePatient;

public sealed record CreatePatientCommand(
    string Email,
    string Password,
    string Phone,
    string Pesel,
    DateOnly DateOfBirth,
    string? FirstName,
    string? LastName,
    string? Insurance,
    PatientAddress Address
) : CreateUserCommand(Email, Password, FirstName, LastName, Phone), IRequest<Result<PatientResponse>>;
