using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace AdCommunity.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public TRepository GetRepository<TRepository>() where TRepository : class
    {
        var repositoryType = typeof(TRepository);
        if (!_repositories.ContainsKey(repositoryType))
        {
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(repositoryType, repositoryInstance);
        }

        return (TRepository)_repositories[repositoryType];
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
