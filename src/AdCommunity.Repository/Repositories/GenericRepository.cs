using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AdCommunity.Repository.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    protected GenericRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    public async Task<T> GetAsync(int id, Func<IQueryable<T>, IQueryable<T>> includeFunc, CancellationToken? cancellationToken = null)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken ?? CancellationToken.None);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IQueryable<T>> includeFunc, CancellationToken? cancellationToken)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = includeFunc(query);

        var result = await query.ToListAsync(cancellationToken ?? CancellationToken.None);

        return result;
    }


    public async Task AddAsync(T entity, CancellationToken? cancellationToken)
    {
        await _dbContext.Set<T>().AddAsync(entity, (CancellationToken)(cancellationToken));
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

}
