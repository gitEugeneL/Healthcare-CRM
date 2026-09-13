using System.Text.Json;
using Application.Abstractions.Cache;
using StackExchange.Redis;

namespace Cache;

internal sealed class CacheService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _db = redis.GetDatabase();
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(5);
    
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        return value.IsNullOrEmpty
            ? default
            : JsonSerializer.Deserialize<T>((string)value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiration ?? _defaultExpiration);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        foreach (var endpoints in redis.GetEndPoints())
        {
            var server = redis.GetServer(endpoints);
            var keys = server.Keys(pattern: $"{prefix}*").ToArray();

            if (keys.Length > 0)
                await _db.KeyDeleteAsync(keys);
        }
    }
}