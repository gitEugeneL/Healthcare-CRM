using Domain.WorkSchedules;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.WorkSchedules;

internal sealed class WorkScheduleRepository(DataContext dataContext) : IWorkScheduleRepository
{
    public async Task InsertWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken ct)
    {
        await dataContext
            .WorkSchedules
            .AddAsync(workSchedule, ct);
    }

    public async Task<WorkSchedule?> FindWorkScheduleByDoctorIdWithTracking(Guid doctorId, CancellationToken ct)
    {
        return await dataContext
            .WorkSchedules
            .SingleOrDefaultAsync(ws => ws.DoctorId == doctorId, ct);
    }

    public async Task<WorkSchedule?> FindWorkScheduleByDoctorId(Guid doctorId, CancellationToken ct)
    {
        return await dataContext
            .WorkSchedules
            .AsNoTracking()
            .SingleOrDefaultAsync(ws => ws.DoctorId == doctorId, ct);
    }
}
