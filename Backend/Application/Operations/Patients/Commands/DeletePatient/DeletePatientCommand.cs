using Application.Common.Models;
using MediatR;

namespace Application.Operations.Patients.Commands.DeletePatient;

public sealed record DeletePatientCommand : CurrentUser, IRequest<Unit>;
