using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    protected GenericRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
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
