using StackExchange.Redis;

namespace AdCommunity.Application.Services;

public interface IRedisService
{
    StackExchange.Redis.IDatabase GetDb(int db);
    Task AddToCacheAsync<T>(string cacheKey, T data,TimeSpan? expireTime=null);
    Task<T> GetFromCacheAsync<T>(string cacheKey);
}
