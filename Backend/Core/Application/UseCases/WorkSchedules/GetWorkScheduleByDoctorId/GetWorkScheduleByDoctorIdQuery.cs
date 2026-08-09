using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.WorkSchedules.GetWorkScheduleByDoctorId;

public sealed record GetWorkScheduleByDoctorIdQuery(Guid DoctorId) : IRequest<Result<WorkScheduleResponse>>;

