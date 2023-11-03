using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.Repositories;

public class UserEventRepository : GenericRepository<UserEvent>, IUserEventRepository
{
    public UserEventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
