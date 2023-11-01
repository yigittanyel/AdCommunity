using AdCommunity.Domain.Entities.UserModels;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class UserCommunityRepository : GenericRepository<UserCommunity>,IUserCommunityRepository
{
    public UserCommunityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
