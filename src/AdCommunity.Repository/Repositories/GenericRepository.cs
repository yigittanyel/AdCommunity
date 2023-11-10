using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    protected GenericRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<T> GetAsync(int id, CancellationToken? cancellationToken)
    {
        return await _dbContext.Set<T>().FindAsync(id,cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken? cancellationToken)
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync((CancellationToken)(cancellationToken));
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
