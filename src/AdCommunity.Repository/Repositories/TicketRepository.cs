using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class TicketRepository : GenericRepository<TicketType>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<TicketType> GetTicketByEventAndCommunityIdsAsync(int eventId, int communityId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.CommunityEventId == eventId && t.CommunityId == communityId, cancellationToken);      
    }
}
