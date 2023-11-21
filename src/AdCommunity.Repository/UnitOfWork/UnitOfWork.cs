using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context,
        IEventRepository eventRepository,
        ICommunityRepository communityRepository,
        ITicketRepository ticketRepository,
        IUserCommunityRepository userCommunityRepository,
        IUserEventRepository userEventRepository,
        IUserTicketRepository userTicketRepository,
        IUserRepository userRepository)
    {
        _context = context;
        EventRepository = eventRepository;
        CommunityRepository = communityRepository;
        TicketRepository = ticketRepository;
        UserCommunityRepository = userCommunityRepository;
        UserEventRepository = userEventRepository;
        UserRepository = userRepository;
        UserTicketRepository = userTicketRepository;
    }

    public IEventRepository EventRepository { get; }
    public ICommunityRepository CommunityRepository { get; }
    public ITicketRepository TicketRepository { get; }
    public IUserCommunityRepository UserCommunityRepository { get; }
    public IUserEventRepository UserEventRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserTicketRepository UserTicketRepository { get; }

    public async Task<int> SaveChangesAsync(CancellationToken? cancellationToken)
    {
        return await _context.SaveChangesAsync((CancellationToken)cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken? cancellationToken)
    {
        await _context.Database.BeginTransactionAsync((CancellationToken)cancellationToken);
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
