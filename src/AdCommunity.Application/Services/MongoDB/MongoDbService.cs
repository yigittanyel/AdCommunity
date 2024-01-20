using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AdCommunity.Application.Services.MongoDB;
public class MongoDbService<T> : IMongoDbService<T>
{
    private readonly IMongoCollection<T> _collection;

    public MongoDbService(IOptions<MongoDbSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _collection = database.GetCollection<T>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.AsQueryable().ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
    }
}

