using Application.UseCases.Common.User;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Patients.UpdatePatient;

public sealed record UpdatePatientCommand(
    Guid PatientId,
    UpdateAddress Address,
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Pesel,
    DateOnly? DateOfBirth,
    string? Insurance
) : UpdateUserCommand(FirstName, LastName, Phone), IRequest<Result<PatientResponse>>;

public sealed record UpdateAddress(
    string? Province,
    string? PostalCode,
    string? City,
    string? Street,
    string? Hose,
    string? Apartment);

