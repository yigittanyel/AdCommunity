namespace AdCommunity.Domain.Repository;

public interface IUnitOfWork
{
    IEventRepository EventRepository { get; }
    ICommunityRepository CommunityRepository { get; }
    ITicketRepository TicketRepository { get; }
    IUserCommunityRepository UserCommunityRepository { get; }
    IUserEventRepository UserEventRepository { get; }
    IUserRepository UserRepository { get; }
    IUserTicketRepository UserTicketRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken? cancellationToken);
    Task BeginTransactionAsync(CancellationToken? cancellationToken);
    Task CommitTransactionAsync(CancellationToken? cancellationToken);
    Task RollbackTransactionAsync(CancellationToken? cancellationToken);
}