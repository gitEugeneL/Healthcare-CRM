using Application.Abstractions.Cache;
using Application.Abstractions.Security.Models;

namespace Security.Services;

public sealed class AttemptLimiter(ICacheService cache)
{
    public async Task<AttemptState> GetStateAsync(string key)
    {
        var state = await cache.GetAsync<AttemptState>(key);
        return IsActive(state) ? state! : new AttemptState();
    }

    public async Task<AttemptState> RegisterAttemptAsync(string key, int maxAttempts, TimeSpan window, bool success)
    {
        var state = await cache.GetAsync<AttemptState>(key);
        state = IsActive(state) ? state! : new AttemptState();

        if (state.LockedUntil.HasValue)
            return state;

        if (success)
        {
            await cache.RemoveAsync(key);
            return new AttemptState();
        }

        var count = state.Count + 1;
        var lockedUntil = count >= maxAttempts ? DateTime.UtcNow.Add(window) : (DateTime?)null;
        var newState = new AttemptState(count, lockedUntil);
        await cache.SetAsync(key, newState, window);
        return newState;
    }

    private static bool IsActive(AttemptState? state) =>
        state is not null && (!state.LockedUntil.HasValue || state.LockedUntil.Value > DateTime.UtcNow);
}