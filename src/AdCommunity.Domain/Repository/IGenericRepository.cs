using System.Linq.Expressions;

namespace AdCommunity.Domain.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetAsync(int id, Func<IQueryable<T>, IQueryable<T>> includeFunc, CancellationToken? cancellationToken = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IQueryable<T>> includeFunc, CancellationToken? cancellationToken);
    Task AddAsync(T entity, CancellationToken? cancellationToken);
    void Delete(T entity);
    void Update(T entity);
}