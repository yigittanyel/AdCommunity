using Newtonsoft.Json;
using StackExchange.Redis;

namespace AdCommunity.Application.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisService(string url)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
    }

    public StackExchange.Redis.IDatabase GetDb(int db)
    {
        return _connectionMultiplexer.GetDatabase(db);
    }

    public async Task AddToCacheAsync<T>(string cacheKey, T data,TimeSpan? expireTime)
    {
        var serializedData = JsonConvert.SerializeObject(data);
        await GetDb(0).StringSetAsync(cacheKey, serializedData,TimeSpan.FromMinutes(1));
    }

    public async Task<T> GetFromCacheAsync<T>(string cacheKey)
    {
        var cachedData = await GetDb(0).StringGetAsync(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        return default(T);
    }

}