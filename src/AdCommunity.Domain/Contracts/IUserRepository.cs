using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Contracts;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersByUsernameAndPasswordAsync(string username, string password);
}
