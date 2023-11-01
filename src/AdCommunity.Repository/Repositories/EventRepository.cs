using AdCommunity.Domain.Entities.CommunityModels;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class EventRepository : GenericRepository<Event>,IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
