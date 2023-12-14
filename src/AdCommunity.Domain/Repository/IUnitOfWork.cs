using Microsoft.EntityFrameworkCore.Storage;

namespace AdCommunity.Domain.Repository;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken? cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken? cancellationToken);
    Task CommitTransactionAsync(CancellationToken? cancellationToken);
    Task RollbackTransactionAsync(CancellationToken? cancellationToken);
}