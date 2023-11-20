using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Repository;

public interface IUserEventRepository : IGenericRepository<UserEvent>
{
    Task<UserEvent> GetUserEventsByUserAndEventAsync(int userId, int eventId, CancellationToken? cancellationToken);
}
