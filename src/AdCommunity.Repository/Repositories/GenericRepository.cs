using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AdCommunity.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await Task.FromCanceled(cancellationToken);
        }

        _context.Set<T>().Update(entity);

        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await Task.FromCanceled(cancellationToken);
        }

        _context.Set<T>().UpdateRange(entities);

        await Task.CompletedTask;
    }

    public async Task<IList<T>> FindManyAsync(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        CancellationToken cancellationToken = default)
    {
        var result = await CreateQuery(predicate, orderBy, include).ToListAsync(cancellationToken);

        return result;
    }

    public async Task<T> FindAsync(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        CancellationToken cancellationToken = default)
    {
        var result = await CreateQuery(predicate, orderBy, include).FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var result = await _context.Set<T>().AnyAsync(predicate, cancellationToken);

        return result;
    }

    public async Task<IList<TResult>> QueryListAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        CancellationToken cancellationToken = default)
    {
        var query = CreateQuery(predicate, orderBy, include);
        var result = await query.Select(selector).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }

    private IQueryable<T> CreateQuery(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query;
    }
}
