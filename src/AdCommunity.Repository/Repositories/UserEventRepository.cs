using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class UserEventRepository : GenericRepository<UserEvent>, IUserEventRepository
{
    public UserEventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserEvent> GetUserEventsByUserAndEventAsync(int userId, int eventId, CancellationToken? cancellationToken)
    {
        return await _dbContext.UserEvents
            .Where(x => x.UserId == userId && x.EventId == eventId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None);
        
    }
}
