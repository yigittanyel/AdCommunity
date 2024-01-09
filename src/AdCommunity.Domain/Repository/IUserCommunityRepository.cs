using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Repository;

public interface IUserCommunityRepository : IGenericRepository<UserCommunity>
{
    Task<UserCommunity> GetUserCommunitiesByUserAndCommunityAsync(int userId, int communityId, CancellationToken cancellationToken);
}
