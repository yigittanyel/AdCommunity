using AdCommunity.Domain.Entities.Aggregates.Community;

namespace AdCommunity.Domain.Repository;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<Ticket> GetTicketByEventAndCommunityIdsAsync(int eventId, int communityId, CancellationToken cancellationToken = default);
}
