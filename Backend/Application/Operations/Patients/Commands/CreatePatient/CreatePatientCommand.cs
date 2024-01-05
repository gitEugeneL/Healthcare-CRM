using Application.Operations.Users.Commands;
using MediatR;

namespace Application.Operations.Patients.Commands.CreatePatient;

public sealed record CreatePatientCommand : CreateUserCommand, IRequest<PatientResponse>;
