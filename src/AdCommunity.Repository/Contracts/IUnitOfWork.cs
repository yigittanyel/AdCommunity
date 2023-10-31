namespace AdCommunity.Repository.Contracts;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}