using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AdCommunity.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var entityType = typeof(TEntity);

        if (_repositories.ContainsKey(entityType))
        {
            return (IGenericRepository<TEntity>)_repositories[entityType];
        }

        var repository = (IGenericRepository<TEntity>)Activator.CreateInstance(typeof(GenericRepository<>).MakeGenericType(entityType), _context);
        _repositories.Add(entityType, repository);
        return repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken? cancellationToken)
    {
        return await _context.SaveChangesAsync((CancellationToken)cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken? cancellationToken = null)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken ?? CancellationToken.None);
    }

    public async Task CommitTransactionAsync(CancellationToken? cancellationToken)
    {
        await _context.Database.CommitTransactionAsync((CancellationToken)cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken? cancellationToken)
    {
        await _context.Database.RollbackTransactionAsync((CancellationToken)cancellationToken);
    }


}
