using AdCommunity.Domain.Entities;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class UserCommunityRepository : GenericRepository<UserCommunity>,IUserCommunityRepository
{
    public UserCommunityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
