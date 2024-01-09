using AdCommunity.Domain.Entities.Aggregates.Community;

namespace AdCommunity.Domain.Repository;

public interface ITicketRepository : IGenericRepository<TicketType>
{
    Task<TicketType> GetTicketByEventAndCommunityIdsAsync(int eventId, int communityId, CancellationToken cancellationToken);
}
