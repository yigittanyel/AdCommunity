using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdCommunity.Application.Services.Redis;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisService(string url)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
    }

    public IDatabase GetDb(int db)
    {
        return _connectionMultiplexer.GetDatabase(db);
    }

    public async Task AddToCacheAsync<T>(string cacheKey, T data, TimeSpan? expireTime)
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var serializedData = System.Text.Json.JsonSerializer.Serialize(data, options);

        var db = GetDb(0);
        await db.StringSetAsync(cacheKey, serializedData);

        if (expireTime.HasValue)
        {
            await db.KeyExpireAsync(cacheKey, expireTime);
        }
    }
    public async Task<T> GetFromCacheAsync<T>(string cacheKey)
    {
        var cachedData = await GetDb(0).StringGetAsync(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return System.Text.Json.JsonSerializer.Deserialize<T>(cachedData, options);
        }

        return default;
    }
}