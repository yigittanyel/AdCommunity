using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Application.Services.MongoDB;

public class MongoDbService<T> : IMongoDbService<T> where T : MongoBaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public MongoDbService(IOptions<MongoDbSettings> mongoDBSettings)
    {
        var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _collection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
