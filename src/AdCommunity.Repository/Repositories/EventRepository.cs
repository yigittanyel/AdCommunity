using AdCommunity.Domain.Contracts;
using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.Repositories;

public class EventRepository : GenericRepository<Event>,IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
