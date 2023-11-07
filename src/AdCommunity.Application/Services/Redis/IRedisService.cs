using StackExchange.Redis;

namespace AdCommunity.Application.Services.Redis;

public interface IRedisService
{
    IDatabase GetDb(int db);
    Task AddToCacheAsync<T>(string cacheKey, T data, TimeSpan? expireTime = null);
    Task<T> GetFromCacheAsync<T>(string cacheKey);
}
