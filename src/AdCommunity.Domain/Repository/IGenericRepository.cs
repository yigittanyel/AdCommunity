namespace AdCommunity.Domain.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetAsync(int id,CancellationToken? cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken? cancellationToken);
    Task AddAsync(T entity, CancellationToken? cancellationToken);
    void Delete(T entity);
    void Update(T entity);
}