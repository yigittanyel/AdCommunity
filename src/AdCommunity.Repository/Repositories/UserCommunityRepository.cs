using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.Repositories;

public class UserCommunityRepository : GenericRepository<UserCommunity>,IUserCommunityRepository
{
    public UserCommunityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
