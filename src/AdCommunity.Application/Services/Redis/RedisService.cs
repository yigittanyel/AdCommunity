using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        await GetDb(0).StringSetAsync(cacheKey, serializedData, TimeSpan.FromMinutes(1));
    }


    public async Task<T> GetFromCacheAsync<T>(string cacheKey)
    {
        var cachedData = await GetDb(0).StringGetAsync(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        return default;
    }

}