using AdCommunity.Domain.Entities.CommunityModels;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class TicketRepository : GenericRepository<Ticket>,ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }
}
