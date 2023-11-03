using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.Repositories;

public class TicketRepository : GenericRepository<Ticket>,ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }
}
