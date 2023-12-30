using MediatR;

namespace Application.Operations.AppointSettings.Queries.GetAppointmentSettings;

public sealed record GetConfigAppointmentQuery(Guid SettingsId) : IRequest<AppointmentSettingsResponse>;
