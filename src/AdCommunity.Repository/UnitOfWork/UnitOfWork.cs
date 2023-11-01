using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context,
        IEventRepository communityEventRepository,
        ICommunityRepository communityRepository,
        ITicketRepository ticketRepository,
        IUserCommunityRepository userCommunityRepository,
        IUserEventRepository userEventRepository,
        IUserTicketRepository userTicketRepository,
        IUserRepository userRepository)
    {
        _context = context;
        CommunityEventRepository = communityEventRepository;
        CommunityRepository = communityRepository;
        TicketRepository = ticketRepository;
        UserCommunityRepository = userCommunityRepository;
        UserEventRepository = userEventRepository;
        UserRepository = userRepository;
        UserTicketRepository = userTicketRepository;
    }

    public IEventRepository CommunityEventRepository { get; }
    public ICommunityRepository CommunityRepository { get; }
    public ITicketRepository TicketRepository { get; }
    public IUserCommunityRepository UserCommunityRepository { get; }
    public IUserEventRepository UserEventRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserTicketRepository UserTicketRepository { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}
