using AdCommunity.Domain.Entities.Aggregates.Community;

namespace AdCommunity.Domain.Repository;

public interface IEventRepository : IGenericRepository<Event>
{
    Task<Event> GetByEventNameAsync(string eventName, CancellationToken cancellationToken);
}
