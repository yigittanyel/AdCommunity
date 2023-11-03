﻿using StackExchange.Redis;

namespace AdCommunity.Application.Services;

public class RedisService
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
}