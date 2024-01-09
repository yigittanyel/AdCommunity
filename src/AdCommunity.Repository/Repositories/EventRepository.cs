using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class EventRepository : GenericRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Event> GetByEventNameAsync(string eventName, CancellationToken cancellationToken)
    {
        return await _dbContext.Events
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.EventName == eventName, cancellationToken);      
    }
}
