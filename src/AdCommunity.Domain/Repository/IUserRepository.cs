using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Repository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersByUsernameAndPasswordAsync(string username, string password);
}
