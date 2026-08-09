using Application.UseCases.Common.User;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Doctors.UpdateDoctor;

public sealed record UpdateDoctorCommand(
    Guid DoctorId,
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Description,
    string? Education
) : UpdateUserCommand(FirstName, LastName, Phone), IRequest<Result<DoctorResponse>>;