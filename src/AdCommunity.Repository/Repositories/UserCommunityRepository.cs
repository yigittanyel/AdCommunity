using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class UserCommunityRepository : GenericRepository<UserCommunity>,IUserCommunityRepository
{
    public UserCommunityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserCommunity> GetUserCommunitiesByUserAndCommunityAsync(int userId, int communityId, CancellationToken? cancellationToken)
    {
        return await _dbContext.UserCommunities
            .Where(x => x.UserId == userId && x.CommunityId == communityId)
            .FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None);   
    }
}
