using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Application.Services.MongoDB;

public interface IMongoDbService<T> where T : MongoBaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}
