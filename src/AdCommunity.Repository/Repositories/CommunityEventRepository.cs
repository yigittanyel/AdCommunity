using AdCommunity.Domain.Entities;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class CommunityEventRepository : GenericRepository<CommunityEvent>,ICommunityEventRepository
{
    public CommunityEventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
