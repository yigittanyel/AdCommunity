namespace AdCommunity.Repository.Contracts;

public interface IUnitOfWork : IDisposable
{
    IEventRepository CommunityEventRepository { get; }
    ICommunityRepository CommunityRepository { get; }
    ITicketRepository TicketRepository { get; }
    IUserCommunityRepository UserCommunityRepository { get; }
    IUserEventRepository UserEventRepository { get; }
    IUserRepository UserRepository { get; }
    IUserTicketRepository UserTicketRepository { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}