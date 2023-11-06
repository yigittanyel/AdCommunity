using AdCommunity.Application.Services;
using Newtonsoft.Json;

namespace AdCommunity.Application.Helpers;

public static class CacheHelper
{
    public static void AddToCache<T>(RedisService redisService, string cacheKey, T data)
    {
        var serializedData = JsonConvert.SerializeObject(data);
        redisService.GetDb(0).StringSet(cacheKey, serializedData);
    }

    public static T GetFromCache<T>(RedisService redisService, string cacheKey)
    {
        var cachedData = redisService.GetDb(0).StringGet(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        return default(T);
    }
}
