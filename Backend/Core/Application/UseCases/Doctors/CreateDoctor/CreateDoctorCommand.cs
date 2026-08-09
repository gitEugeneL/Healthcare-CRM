using Application.UseCases.Common.User;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Doctors.CreateDoctor;

public sealed record CreateDoctorCommand(
    string Email,
    string Password,
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Education,
    string? Description
) : CreateUserCommand(Email, Password, FirstName, LastName, Phone), IRequest<Result<DoctorResponse>>;
