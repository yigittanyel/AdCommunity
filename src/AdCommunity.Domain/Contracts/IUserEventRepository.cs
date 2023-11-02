using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Contracts;

public interface IUserEventRepository : IGenericRepository<UserEvent>
{
}
