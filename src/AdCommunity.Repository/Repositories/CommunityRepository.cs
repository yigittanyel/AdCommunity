using AdCommunity.Domain.Contracts;
using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.Repositories;

public class CommunityRepository : GenericRepository<Community>, ICommunityRepository   
{
    public CommunityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
