using AdCommunity.Domain.Entities.Aggregates.Community;

namespace AdCommunity.Domain.Repository;

public interface ICommunityRepository : IGenericRepository<Community>
{
   Task<Community> GetByCommunityNameAsync(string communityName, CancellationToken? cancellationToken);  

    Task<IEnumerable<Community>> GetAllWithIncludeAsync(CancellationToken? cancellationToken);
}
